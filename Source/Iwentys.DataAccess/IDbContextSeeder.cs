using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess;

public interface IDbContextSeeder
{
    void Seed(ModelBuilder modelBuilder);
}