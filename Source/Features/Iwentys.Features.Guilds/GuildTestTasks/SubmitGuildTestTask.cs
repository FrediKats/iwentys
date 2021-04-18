﻿using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using MediatR;

namespace Iwentys.Features.Guilds.GuildTestTasks
{
    public static class SubmitGuildTestTask
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

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly GithubIntegrationService _githubIntegrationService;
            private readonly IGenericRepository<Guild> _guildRepository;
            private readonly IGenericRepository<GuildTestTaskSolution> _guildTestTaskSolutionRepository;
            private readonly IGenericRepository<ProjectReviewRequest> _projectReviewRequestRepository;
            private readonly IGenericRepository<IwentysUser> _userRepository;

            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork, AchievementProvider achievementProvider, GithubIntegrationService githubIntegrationService)
            {
                _githubIntegrationService = githubIntegrationService;

                _unitOfWork = unitOfWork;
                _userRepository = _unitOfWork.GetRepository<IwentysUser>();
                _projectReviewRequestRepository = _unitOfWork.GetRepository<ProjectReviewRequest>();
                _guildRepository = _unitOfWork.GetRepository<Guild>();
                _guildTestTaskSolutionRepository = _unitOfWork.GetRepository<GuildTestTaskSolution>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Guild guild = await _guildRepository.GetById(request.GuildId);
                IwentysUser user = await _userRepository.GetById(request.User.Id);
                GithubRepositoryInfoDto githubRepositoryInfoDto = _githubIntegrationService.Repository.GetRepository(request.ProjectOwner, request.ProjectName).Result;
                GuildTestTaskSolution testTaskSolution = await _guildTestTaskSolutionRepository.GetSingle(t => t.AuthorId == request.User.Id && t.GuildId == request.GuildId);

                ProjectReviewRequest reviewRequest = ProjectReviewRequest.CreateGuildReviewRequest(user, githubRepositoryInfoDto, testTaskSolution, guild);

                _guildTestTaskSolutionRepository.Update(testTaskSolution);
                _projectReviewRequestRepository.Insert(reviewRequest);
                await _unitOfWork.CommitAsync();
                return new Response(GuildTestTaskInfoResponse.Wrap(testTaskSolution));
            }
        }
    }
}