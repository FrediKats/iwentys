using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess.Seeding;

public interface IEntityGenerator
{
    void Seed(ModelBuilder modelBuilder);
}