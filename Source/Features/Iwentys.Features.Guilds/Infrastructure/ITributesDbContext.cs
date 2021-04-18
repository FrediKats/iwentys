using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Infrastructure
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