using Iwentys.Domain.Guilds;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Subcontext
{
    public interface IGuildsTournamentsDbContext
    {
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentParticipantTeam> TournamentParticipantTeams { get; set; }
        public DbSet<TournamentTeamMember> TournamentTeamMembers { get; set; }
        public DbSet<CodeMarathonTournament> CodeMarathonTournaments { get; set; }
    }

    public static class GuildsTournamentsDbContextExtensions
    {
        public static void OnGuildsTournamentsModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TournamentTeamMember>().HasKey(g => new { g.TeamId, g.MemberId });
        }
    }
}