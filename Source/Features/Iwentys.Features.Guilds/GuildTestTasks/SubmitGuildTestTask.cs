using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain;
using Iwentys.Domain.Enums;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using Iwentys.Features.PeerReview.Services;
using MediatR;

namespace Iwentys.Features.Guilds.GuildTestTasks
{
    public class SubmitGuildTestTask
    {
        public class Query : IRequest<Response>
        {
            public Query(int guildId, AuthorizedUser user)
            {
                GuildId = guildId;
                User = user;
            }

            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }
            public string ProjectOwner { get; set; }
            public string ProjectName { get; set; }
        }

        public class Response
        {
            public Response(GuildTestTaskInfoResponse testTaskInfo)
            {
                TestTaskInfo = testTaskInfo;
            }

            public GuildTestTaskInfoResponse TestTaskInfo { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly AchievementProvider _achievementProvider;
            private readonly GithubIntegrationService _githubIntegrationService;

            private readonly IGenericRepository<GuildMember> _guildMemberRepository;
            private readonly IGenericRepository<Guild> _guildRepository;
            private readonly IGenericRepository<GuildTestTaskSolution> _guildTestTaskSolutionRepository;

            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepository<IwentysUser> _userRepository;

            private readonly ProjectReviewService _projectReviewService;

            public Handler(IUnitOfWork unitOfWork, AchievementProvider achievementProvider, GithubIntegrationService githubIntegrationService, ProjectReviewService projectReviewService)
            {
                _achievementProvider = achievementProvider;
                _githubIntegrationService = githubIntegrationService;
                _projectReviewService = projectReviewService;

                _unitOfWork = unitOfWork;
                _userRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepository = _unitOfWork.GetRepository<Guild>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
                _guildTestTaskSolutionRepository = _unitOfWork.GetRepository<GuildTestTaskSolution>();
            }

            protected override Response Handle(Query request)
            {
                //TODO: Add exception message (Test task was not started)
                GuildTestTaskSolution testTaskSolution = _guildTestTaskSolutionRepository
                    .GetSingle(t => t.AuthorId == request.User.Id && t.GuildId == request.GuildId).Result;

                if (testTaskSolution.GetState() == GuildTestTaskState.Completed)
                    throw new InnerLogicException("Task already completed");

                Guild guild = _guildRepository.GetById(request.GuildId).Result;
                GithubRepositoryInfoDto githubRepositoryInfoDto = _githubIntegrationService.Repository.GetRepository(request.ProjectOwner, request.ProjectName).Result;
                var createArguments = new ReviewRequestCreateArguments
                {
                    ProjectId = githubRepositoryInfoDto.Id,
                    Description = "Guild test task review",
                    Visibility = ProjectReviewVisibility.Closed
                };

                //TODO: here we call .Commit and... it's not okay
                ProjectReviewRequestInfoDto reviewRequest = _projectReviewService.CreateReviewRequest(request.User, createArguments).Result;

                foreach (GuildMember member in guild.Members)
                {
                    if (member.MemberId == request.User.Id)
                        continue;

                    if (!member.MemberType.IsMentor())
                        continue;

                    _projectReviewService.InviteToReview(request.User, reviewRequest.Id, member.MemberId).Wait();
                }

                testTaskSolution.SendSubmit(request.User, reviewRequest);

                _guildTestTaskSolutionRepository.Update(testTaskSolution);
                _unitOfWork.CommitAsync().Wait();
                return new Response(GuildTestTaskInfoResponse.Wrap(testTaskSolution));
            }
        }
    }
}