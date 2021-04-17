using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.GuildTestTasks
{
    public class AcceptGuildTestTask
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
                Guild authorGuild = _guildMemberRepository.ReadForStudent(request.User.Id);
                if (authorGuild is null || authorGuild.Id != request.GuildId)
                    throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, request.GuildId);

                IwentysUser author = _userRepository.FindByIdAsync(request.User.Id).Result;

                GuildTestTaskSolution existedTestTaskSolution = _guildTestTaskSolutionRepository
                    .Get()
                    .Where(GuildTestTaskSolution.IsNotCompleted)
                    .FirstOrDefaultAsync(k =>
                        k.GuildId == authorGuild.Id &&
                        k.AuthorId == request.User.Id).Result;

                if (existedTestTaskSolution is not null)
                    InnerLogicException.GuildExceptions.ActiveTestExisted(request.User.Id, request.GuildId);

                var testTaskSolution = GuildTestTaskSolution.Create(authorGuild, author);

                _guildTestTaskSolutionRepository.InsertAsync(testTaskSolution).Wait();
                _unitOfWork.CommitAsync().Wait();
                return new Response(GuildTestTaskInfoResponse.Wrap(testTaskSolution));
            }
        }
    }
}