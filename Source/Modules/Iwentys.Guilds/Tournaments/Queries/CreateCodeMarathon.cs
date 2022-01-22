using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public static class CreateCodeMarathon
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser user, CreateCodeMarathonTournamentArguments arguments)
        {
            User = user;
            Arguments = arguments;
        }

        public AuthorizedUser User { get; set; }
        public CreateCodeMarathonTournamentArguments Arguments { get; set; }
    }

    public class Response
    {
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id);

            var codeMarathonTournamentEntity = CodeMarathonTournament.Create(user, request.Arguments);

            _context.CodeMarathonTournaments.Add(codeMarathonTournamentEntity);

            return new Response();
        }
    }
}