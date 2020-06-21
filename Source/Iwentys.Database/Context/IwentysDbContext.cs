using Iwentys.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Context
{
    public class IwentysDbContext : DbContext
    {
        public DbSet<UserProfile> UserProfile { get; set; }

        public IwentysDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}