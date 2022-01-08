using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Guilds
{
    public static class RegisterToTournament
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
            public CreateCodeMarathonTournamentArguments Arguments { get; set; }
        }

        public class Response
        {
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            protected override Response Handle(Query request)
            {
                IwentysUser studentEntity = _context.IwentysUsers.GetById(request.User.Id).Result;
                Guild guild = _context.GuildMembers.ReadForStudent(request.User.Id).Result;
                Tournament tournamentEntity = _context.Tournaments.GetById(request.TournamentId).Result;

                TournamentParticipantTeam tournamentParticipantTeamEntity = tournamentEntity.RegisterTeam(studentEntity, guild);

                _context.TournamentParticipantTeams.Add(tournamentParticipantTeamEntity);
                return new Response();
            }
        }
    }
}