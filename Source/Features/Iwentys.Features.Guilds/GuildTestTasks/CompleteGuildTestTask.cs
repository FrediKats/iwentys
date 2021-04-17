using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using Iwentys.Features.PeerReview.Services;
using MediatR;

namespace Iwentys.Features.Guilds.GuildTestTasks
{
    public class CompleteGuildTestTask
    {
        public class Query : IRequest<Response>
        {
            public Query(int guildId, AuthorizedUser user, int taskSolveOwnerId)
            {
                GuildId = guildId;
                User = user;
                TaskSolveOwnerId = taskSolveOwnerId;
            }

            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }
            public int TaskSolveOwnerId { get; set; }
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
                IwentysUser review = _userRepository.FindByIdAsync(request.User.Id).Result;
                review.EnsureIsGuildMentor(_guildRepository, request.GuildId).Wait();

                GuildTestTaskSolution testTask = _guildTestTaskSolutionRepository
                    .GetSingle(t => t.AuthorId == request.TaskSolveOwnerId && t.GuildId == request.GuildId)
                    .Result;

                if (testTask.GetState() != GuildTestTaskState.Submitted)
                    throw new InnerLogicException("Task must be submitted");

                testTask.SetCompleted(review);
                _achievementProvider.Achieve(AchievementList.TestTaskDone, request.TaskSolveOwnerId).Wait();

                _guildTestTaskSolutionRepository.Update(testTask);
                _unitOfWork.CommitAsync().Wait();
                return new Response(GuildTestTaskInfoResponse.Wrap(testTask));
            }
        }
    }
}