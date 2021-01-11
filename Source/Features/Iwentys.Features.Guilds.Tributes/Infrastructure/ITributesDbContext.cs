using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Tributes.Infrastructure
{
    public interface ITributesDbContext
    {
        
    }

    public static class DbContextExtensions
    {
        public static void OnTributesModelCreating(this ModelBuilder modelBuilder)
        {
        }
    }
}