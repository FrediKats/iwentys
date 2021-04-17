using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using MediatR;

namespace Iwentys.Features.Guilds.Guilds
{
    public class UpdateGuild
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, GuildUpdateRequestDto arguments)
            {
                AuthorizedUser = authorizedUser;
                Arguments = arguments;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
            public GuildUpdateRequestDto Arguments { get; set; }
        }

        public class Response
        {
            public Response(GuildProfileShortInfoDto guild)
            {
                Guild = guild;
            }

            public GuildProfileShortInfoDto Guild { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly GithubIntegrationService _githubIntegrationService;

            private readonly IGenericRepository<GuildMember> _guildMemberRepository;
            private readonly IGenericRepository<GuildPinnedProject> _guildPinnedProjectRepository;
            private readonly IGenericRepository<Guild> _guildRepository;
            private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;

            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
            {
                _githubIntegrationService = githubIntegrationService;

                _unitOfWork = unitOfWork;
                _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepository = _unitOfWork.GetRepository<Guild>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
                _guildPinnedProjectRepository = _unitOfWork.GetRepository<GuildPinnedProject>();
            }

            protected override Response Handle(Query request)
            {
                Guild guild = _guildRepository.GetById(request.Arguments.Id).Result;
                GuildMentor guildMentor = _iwentysUserRepository.GetById(request.AuthorizedUser.Id).EnsureIsGuildMentor(guild).Result;

                guild.Update(guildMentor, request.Arguments);

                _guildRepository.Update(guild);
                _unitOfWork.CommitAsync().Wait();
                return new Response(new GuildProfileShortInfoDto(guild));
            }
        }
    }
}