using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Subcontext
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