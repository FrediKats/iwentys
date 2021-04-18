using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Models;
using Iwentys.Features.GithubIntegration.GithubIntegration;
using MediatR;

namespace Iwentys.Features.Guilds.GuildTributes
{
    public class GetGuildTributes
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, int guildId)
            {
                User = user;
                GuildId = guildId;
            }

            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }
        }

        public class Response
        {
            public Response(List<TributeInfoResponse> tribute)
            {
                Tribute = tribute;
            }

            public List<TributeInfoResponse> Tribute { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly GithubIntegrationService _githubIntegrationService;
            private readonly IGenericRepository<GuildMember> _guildMemberRepository;
            private readonly IGenericRepository<Guild> _guildRepositoryNew;
            private readonly IGenericRepository<Tribute> _guildTributeRepository;
            private readonly IGenericRepository<GithubProject> _studentProjectRepository;

            private readonly IGenericRepository<IwentysUser> _studentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
            {
                _githubIntegrationService = githubIntegrationService;
                _unitOfWork = unitOfWork;
                _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepositoryNew = _unitOfWork.GetRepository<Guild>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
                _studentProjectRepository = _unitOfWork.GetRepository<GithubProject>();
                _guildTributeRepository = _unitOfWork.GetRepository<Tribute>();
            }

            protected override Response Handle(Query request)
            {
                List<TributeInfoResponse> responses = _guildTributeRepository
                    .Get()
                    .Where(t => t.GuildId == request.GuildId)
                    .Where(t => t.State == TributeState.Active)
                    .Select(TributeInfoResponse.FromEntity)
                    .ToList();

                return new Response(responses);
            }
        }
    }
}