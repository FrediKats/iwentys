using Iwentys.Domain.AccountManagement;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess
{
    public interface IAccountManagementDbContext
    {
        public DbSet<UniversitySystemUser> UniversitySystemUsers { get; set; }
        public DbSet<IwentysUser> IwentysUsers { get; set; }
    }
}