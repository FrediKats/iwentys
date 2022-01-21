using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.DataAccess;

public interface IDbContextSeeder
{
    void Seed(ModelBuilder modelBuilder);
}

public class EmptyDbContextSeeder : IDbContextSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
    }
}