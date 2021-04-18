using Iwentys.Domain.Gamification;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Gamification.Infrastructure
{
    public interface IInterestTagsDbContext
    {
        public DbSet<InterestTag> InterestTags { get; set; }
        public DbSet<UserInterestTag> UserInterestTags { get; set; }
        //TODO: move to .Raids?
        public DbSet<RaidInterestTag> RaidInterestTags { get; set; }

    }

    public static class InterestTagsDbContextExtensions
    {
        public static void OnInterestTagsModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInterestTag>().HasKey(g => new { StudentId = g.UserId, g.InterestTagId });
            modelBuilder.Entity<RaidInterestTag>().HasKey(rv => new { rv.RaidId, rv.InterestTagId });
        }
    }
}