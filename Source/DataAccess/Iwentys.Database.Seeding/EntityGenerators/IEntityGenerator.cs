using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public interface IEntityGenerator
    {
        void Seed(ModelBuilder modelBuilder);
    }
}