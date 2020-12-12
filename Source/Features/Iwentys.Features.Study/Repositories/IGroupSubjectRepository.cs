using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;

namespace Iwentys.Features.Study.Repositories
{
    public interface IGroupSubjectRepository : IRepository<GroupSubjectEntity, int>
    {
        IQueryable<SubjectEntity> GetSubjectsForDto(StudySearchParametersDto searchParametersDto);
        IQueryable<StudyGroupEntity> GetStudyGroupsForDto(int? courseId);
        GroupSubjectEntity Create(GroupSubjectEntity entity);
    }
}