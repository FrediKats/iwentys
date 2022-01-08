using Iwentys.Domain.Newsfeeds;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess
{
    public interface INewsfeedsDbContext
    {
        public DbSet<Newsfeed> Newsfeeds { get; set; }
        public DbSet<SubjectNewsfeed> SubjectNewsfeeds { get; set; }
        public DbSet<GuildNewsfeed> GuildNewsfeeds { get; set; }
    }

    public static class NewsfeedsDbContextExtensions
    {
        public static void OnNewsfeedsModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectNewsfeed>().HasKey(g => new { g.SubjectId, g.NewsfeedId });
            modelBuilder.Entity<GuildNewsfeed>().HasKey(g => new { g.GuildId, g.NewsfeedId });
        }
    }
}