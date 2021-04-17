using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.GuildTestTasks
{
    public class GetGuildTestTaskSubmits
    {
        public class Query : IRequest<Response>
        {
            public Query(int guildId)
            {
                GuildId = guildId;
            }

            public int GuildId { get; set; }
        }

        public class Response
        {
            public Response(List<GuildTestTaskInfoResponse> submits)
            {
                Submits = submits;
            }

            public List<GuildTestTaskInfoResponse> Submits { get; set; }
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

            public Handler(IUnitOfWork unitOfWork, AchievementProvider achievementProvider, GithubIntegrationService githubIntegrationService)
            {
                _achievementProvider = achievementProvider;
                _githubIntegrationService = githubIntegrationService;

                _unitOfWork = unitOfWork;
                _userRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepository = _unitOfWork.GetRepository<Guild>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
                _guildTestTaskSolutionRepository = _unitOfWork.GetRepository<GuildTestTaskSolution>();
            }

            protected override Response Handle(Query request)
            {
                List<GuildTestTaskInfoResponse> result = _guildTestTaskSolutionRepository
                    .Get()
                    .Where(t => t.GuildId == request.GuildId)
                    .Select(GuildTestTaskInfoResponse.FromEntity)
                    .ToListAsync().Result;

                return new Response(result);
            }
        }
    }
}