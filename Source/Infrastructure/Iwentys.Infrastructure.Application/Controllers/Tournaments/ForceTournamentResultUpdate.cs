using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.Application.Controllers.Services;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Tournaments
{
    public static class ForceTournamentResultUpdate
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, int tournamentId)
            {
                User = user;
                TournamentId = tournamentId;
            }

            public AuthorizedUser User { get; set; }
            public int TournamentId { get; set; }
        }

        public class Response
        {
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly GithubIntegrationService _githubIntegrationService;
            private readonly IGenericRepository<Tournament> _tournamentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
            {
                _unitOfWork = unitOfWork;

                _tournamentRepository = _unitOfWork.GetRepository<Tournament>();
                _githubIntegrationService = githubIntegrationService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Tournament tournamentEntity = await _tournamentRepository.GetById(request.TournamentId);

                ITournamentDomain tournamentDomain = tournamentEntity.WrapToDomain(_githubIntegrationService);
                tournamentDomain.UpdateResult();

                _tournamentRepository.Update(tournamentEntity);
                await _unitOfWork.CommitAsync();
                return new Response();
            }
        }
    }
}