using Iwentys.Domain.Gamification;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Subcontext
{
    public interface IEconomyDbContext
    {
        public DbSet<BarsPointTransaction> BarsPointTransactionLogs { get; set; }
    }

    public static class EconomyDbContextExtensions
    {
        public static void OnEconomyModelCreating(this ModelBuilder modelBuilder)
        {
        }
    }
}