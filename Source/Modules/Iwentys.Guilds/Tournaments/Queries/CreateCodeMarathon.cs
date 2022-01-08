using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
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

        public Handler(IwentysDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser user = await _context.IwentysUsers.GetById(request.User.Id);

            var codeMarathonTournamentEntity = CodeMarathonTournament.Create(user, request.Arguments);

            _context.CodeMarathonTournaments.Add(codeMarathonTournamentEntity);

            return new Response();
        }
    }
}