using Iwentys.Features.AccountManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.AccountManagement.Infrastructure
{
    public interface IAccountManagementDbContext
    {
        public DbSet<UniversitySystemUser> UniversitySystemUsers { get; set; }
        public DbSet<IwentysUser> IwentysUsers { get; set; }
    }

    public static class DbContextExtensions
    {
        public static void OnAccountManagementModelCreating(this ModelBuilder modelBuilder)
        {

        }
    }
}