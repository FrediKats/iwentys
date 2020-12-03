using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.ViewModels;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface IGroupSubjectRepository : IGenericRepository<GroupSubjectEntity, int>
    {
        IQueryable<SubjectEntity> GetSubjectsForDto(StudySearchParameters searchParameters);
        IQueryable<StudyGroupEntity> GetStudyGroupsForDto(int? courseId);
        GroupSubjectEntity Create(GroupSubjectEntity entity);
    }
}