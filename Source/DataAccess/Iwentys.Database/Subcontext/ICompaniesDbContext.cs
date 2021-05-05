using Iwentys.Domain.Companies;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Subcontext
{
    public interface ICompaniesDbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyWorker> CompanyWorkers { get; set; }
    }

    public static class CompaniesDbContextExtensions
    {
        public static void OnCompaniesModelCreating(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyWorker>().HasKey(g => new { g.CompanyId, g.WorkerId });

            modelBuilder.Entity<CompanyWorker>().HasIndex(g => g.WorkerId).IsUnique();
        }
    }
}