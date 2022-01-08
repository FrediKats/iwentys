using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.Guilds;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

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
        private readonly IwentysDbContext _context;
        private readonly GithubIntegrationService _githubIntegrationService;

        public Handler(IwentysDbContext context, GithubIntegrationService githubIntegrationService)
        {
            _context = context;
            _githubIntegrationService = githubIntegrationService;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            Tournament tournamentEntity = await _context.Tournaments.GetById(request.TournamentId);

            ITournamentDomain tournamentDomain = tournamentEntity.WrapToDomain(_githubIntegrationService);
            tournamentDomain.UpdateResult();

            _context.Tournaments.Update(tournamentEntity);
            return new Response();
        }
    }
}