using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Guilds
{
    public class GetGuildById
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
            public Response(GuildProfileDto guild)
            {
                Guild = guild;
            }

            public GuildProfileDto Guild { get; set; }
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
                GuildProfileDto guildProfileDto = _guildRepository
                    .Get()
                    .Where(g => g.Id == request.GuildId)
                    .Select(GuildProfileDto.FromEntity)
                    .SingleAsync().Result;

                return new Response(guildProfileDto);
            }
        }
    }
}