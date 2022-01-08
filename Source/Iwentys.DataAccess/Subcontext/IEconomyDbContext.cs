using Iwentys.Domain.Gamification;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess;

public interface IEconomyDbContext
{
    public DbSet<BarsPointTransaction> BarsPointTransactionLogs { get; set; }
}