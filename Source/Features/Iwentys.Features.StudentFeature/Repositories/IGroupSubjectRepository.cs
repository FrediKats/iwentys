using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface IGroupSubjectRepository : IGenericRepository<GroupSubjectEntity, int>
    {
        IQueryable<SubjectEntity> GetSubjectsForDto(StudySearchParameters searchParameters);
        IQueryable<StudyGroupEntity> GetStudyGroupsForDto(int? courseId);
        GroupSubjectEntity Create(GroupSubjectEntity entity);
    }
}