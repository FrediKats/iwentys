using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudyGroupRepository : IGenericRepository<StudyGroupEntity, int>
    {
        StudyGroupEntity ReadByNamePattern(string groupName);
    }
}