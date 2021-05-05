using Iwentys.Domain.AccountManagement;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Subcontext
{
    public interface IAccountManagementDbContext
    {
        public DbSet<UniversitySystemUser> UniversitySystemUsers { get; set; }
        public DbSet<IwentysUser> IwentysUsers { get; set; }
    }

    public static class AccountManagementDbContextExtensions
    {
        public static void OnAccountManagementModelCreating(this ModelBuilder modelBuilder)
        {

        }
    }
}