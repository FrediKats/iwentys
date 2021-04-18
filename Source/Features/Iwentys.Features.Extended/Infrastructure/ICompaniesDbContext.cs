using Iwentys.Domain;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.Infrastructure
{
    public interface ICompaniesDbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyWorker> CompanyWorkers { get; set; }
    }

    public static class CompanyDbContextExtensions
    {
        public static void OnCompaniesModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyWorker>().HasKey(g => new { g.CompanyId, g.WorkerId });

            modelBuilder.Entity<CompanyWorker>().HasIndex(g => g.WorkerId).IsUnique();
        }
    }
}