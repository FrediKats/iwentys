using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using MediatR;

namespace Iwentys.Features.Guilds.Guilds
{
    public class GetGuildRating
    {
        public class Query : IRequest<Response>
        {
            public Query(int skip, int take)
            {
                Skip = skip;
                Take = take;
            }

            public int Skip { get; set; }
            public int Take { get; set; }
        }

        public class Response
        {
            public Response(List<GuildProfileDto> guilds)
            {
                Guilds = guilds;
            }

            public List<GuildProfileDto> Guilds { get; set; }
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
                List<GuildProfileDto> result = _guildRepository
                    .Get()
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .Select(GuildProfileDto.FromEntity)
                    .ToList()
                    .OrderByDescending(g => g.GuildRating)
                    .ToList();

                return new Response(result);
            }
        }
    }
}