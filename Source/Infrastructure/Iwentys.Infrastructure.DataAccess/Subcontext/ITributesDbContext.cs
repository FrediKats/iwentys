using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Subcontext
{
    public interface ITributesDbContext
    {
        
    }

    public static class TributeDbContextExtensions
    {
        public static void OnTributesModelCreating(this ModelBuilder modelBuilder)
        {
        }
    }
}