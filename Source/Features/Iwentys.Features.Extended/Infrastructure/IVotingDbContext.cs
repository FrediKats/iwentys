using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.Infrastructure
{
    public interface IVotingDbContext
    {
        
    }

    public static class DbContextExtensions
    {
        public static void OnVotingModelCreating(this ModelBuilder modelBuilder)
        {
        }
    }
}