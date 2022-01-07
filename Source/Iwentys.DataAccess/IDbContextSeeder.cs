using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess
{
    public interface IDbContextSeeder
    {
        void Seed(ModelBuilder modelBuilder);
    }
}
