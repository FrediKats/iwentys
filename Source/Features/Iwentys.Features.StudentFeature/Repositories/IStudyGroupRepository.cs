using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Tools;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface IStudyGroupRepository : IGenericRepository<StudyGroupEntity, int>
    {
        StudyGroupEntity ReadByNamePattern(GroupName group);
        StudyGroupEntity Create(StudyGroupEntity entity);
    }
}