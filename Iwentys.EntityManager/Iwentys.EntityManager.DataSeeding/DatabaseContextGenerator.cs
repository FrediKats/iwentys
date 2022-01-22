using Iwentys.EntityManager.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.DataSeeding;

public class DatabaseContextGenerator : IDbContextSeeder
{
    private readonly List<IEntityGenerator> _generators;

    public DatabaseContextGenerator()
    {
        _generators = new List<IEntityGenerator>();
        StudyEntitiesGenerator studyEntitiesGenerator = Register(new StudyEntitiesGenerator());
        Register(new StudentGenerator(studyEntitiesGenerator.StudyGroups));
    }

    public void Seed(ModelBuilder modelBuilder)
    {
        _generators.ForEach(eg => eg.Seed(modelBuilder));
    }

    private T Register<T>(T entityGenerator) where T : IEntityGenerator
    {
        _generators.Add(entityGenerator);
        return entityGenerator;
    }
}