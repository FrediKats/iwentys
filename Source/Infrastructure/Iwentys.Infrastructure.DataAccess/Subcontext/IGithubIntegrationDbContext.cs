using Iwentys.Domain.GithubIntegration;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Subcontext
{
    public interface IGithubIntegrationDbContext
    {
        public DbSet<GithubProject> StudentProjects { get; set; }
        public DbSet<GithubUser> GithubUsersData { get; set; }
    }

    public static class GithubIntegrationDbContextExtensions
    {
        public static void OnGithubIntegrationModelCreating(this ModelBuilder modelBuilder)
        {
        }
    }
}