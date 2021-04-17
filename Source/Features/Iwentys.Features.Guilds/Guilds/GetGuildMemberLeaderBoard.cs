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
    public class GetGuildMemberLeaderBoard
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
            public Response(GuildMemberLeaderBoardDto guildMemberLeaderBoard)
            {
                GuildMemberLeaderBoard = guildMemberLeaderBoard;
            }

            public GuildMemberLeaderBoardDto GuildMemberLeaderBoard { get; set; }
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
                Guild guild = _guildRepository.GetById(request.GuildId).Result;

                return new Response(new GuildMemberLeaderBoardDto(guild.GetImpact()));
            }
        }
    }
}