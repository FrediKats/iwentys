using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.GithubIntegration;
using Iwentys.Infrastructure.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.GuildTributes
{
    public class GetGuildTributeById
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, int tributeId)
            {
                User = user;
                TributeId = tributeId;
            }

            public AuthorizedUser User { get; set; }
            public int TributeId { get; set; }
        }

        public class Response
        {
            public Response(TributeInfoResponse tribute)
            {
                Tribute = tribute;
            }

            public TributeInfoResponse Tribute { get; set; }
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
                Guild guild = _guildMemberRepository.ReadForStudent(request.User.Id) ?? throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, null);

                TributeInfoResponse result = _guildTributeRepository
                    .Get()
                    .Where(t => t.ProjectId == request.TributeId)
                    .Select(TributeInfoResponse.FromEntity)
                    .FirstAsync().Result;

                return new Response(result);
            }
        }
    }
}