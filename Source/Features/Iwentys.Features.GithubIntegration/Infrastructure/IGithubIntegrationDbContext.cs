using Iwentys.Domain.GithubIntegration;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.GithubIntegration.Infrastructure
{
    public interface IGithubIntegrationDbContext
    {
        public DbSet<GithubProject> StudentProjects { get; set; }
        public DbSet<GithubUser> GithubUsersData { get; set; }
    }

    public static class DbContextExtensions
    {
        public static void OnGithubIntegrationModelCreating(this ModelBuilder modelBuilder)
        {
        }
    }
}