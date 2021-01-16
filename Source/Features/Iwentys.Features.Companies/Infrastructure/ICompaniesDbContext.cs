using Iwentys.Features.Companies.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Companies.Infrastructure
{
    public interface ICompaniesDbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyWorker> CompanyWorkers { get; set; }
    }

    public static class DbContextExtensions
    {
        public static void OnCompaniesModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyWorker>().HasKey(g => new { g.CompanyId, g.WorkerId });

            modelBuilder.Entity<CompanyWorker>().HasIndex(g => g.WorkerId).IsUnique();
        }
    }
}