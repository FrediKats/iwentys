using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Seeding.EntityGenerators
{
    public interface IEntityGenerator
    {
        void Seed(ModelBuilder modelBuilder);
    }
}