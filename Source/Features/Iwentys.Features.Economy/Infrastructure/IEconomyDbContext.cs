using Iwentys.Features.Economy.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Economy.Infrastructure
{
    public interface IEconomyDbContext
    {
        public DbSet<BarsPointTransaction> BarsPointTransactionLogs { get; set; }
    }

    public static class DbContextExtensions
    {
        public static void OnEconomyModelCreating(this ModelBuilder modelBuilder)
        {
        }
    }
}