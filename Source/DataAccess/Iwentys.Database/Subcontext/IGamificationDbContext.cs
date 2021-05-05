using Iwentys.Domain.Gamification;
using Iwentys.Domain.Karmas;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Subcontext
{
    public interface IGamificationDbContext
    {
        public DbSet<KarmaUpVote> KarmaUpVotes { get; set; }
        public DbSet<CourseLeaderboardRow> CourseLeaderboardRows { get; set; }
    }

    public static class GamificationDbContextExtensions
    {
        public static void OnGamificationModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KarmaUpVote>().HasKey(g => new { g.AuthorId, g.TargetId });
            modelBuilder.Entity<CourseLeaderboardRow>().HasKey(clr => new { clr.CourseId, clr.Position });
        }
    }
}