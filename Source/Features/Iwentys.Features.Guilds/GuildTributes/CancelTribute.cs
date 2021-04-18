using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using MediatR;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    public class CancelTribute
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, long tributeId)
            {
                User = user;
                TributeId = tributeId;
            }

            public AuthorizedUser User { get; set; }
            public long TributeId { get; set; }
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
                IwentysUser student = _studentRepository.GetById(request.User.Id).Result;
                Tribute tribute = _guildTributeRepository.GetById(request.TributeId).Result;

                if (tribute.Project.OwnerUserId == request.User.Id)
                {
                    tribute.SetCanceled();
                }
                else
                {
                    student.EnsureIsGuildMentor(_guildRepositoryNew, tribute.GuildId).Wait();
                    tribute.SetCanceled();
                }

                _guildTributeRepository.Update(tribute);
                _unitOfWork.CommitAsync().Wait();
                return new Response(TributeInfoResponse.Wrap(tribute));
            }
        }
    }
}