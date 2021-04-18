using Iwentys.Domain;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.Infrastructure
{
    public interface INewsfeedsDbContext
    {
        public DbSet<Newsfeed> Newsfeeds { get; set; }
        public DbSet<SubjectNewsfeed> SubjectNewsfeeds { get; set; }
        public DbSet<GuildNewsfeed> GuildNewsfeeds { get; set; }
    }

    public static class NewsfeedDbContextExtensions
    {
        public static void OnNewsfeedsModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectNewsfeed>().HasKey(g => new { g.SubjectId, g.NewsfeedId });
            modelBuilder.Entity<GuildNewsfeed>().HasKey(g => new { g.GuildId, g.NewsfeedId });
        }
    }
}