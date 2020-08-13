using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudyGroupRepository : IGenericRepository<StudyGroup, int>
    {
        StudyGroup ReadByNamePattern(string namePattern);
    }
}