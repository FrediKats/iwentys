using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.DataSeeding;

public interface IEntityGenerator
{
    void Seed(ModelBuilder modelBuilder);
}