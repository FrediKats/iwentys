using System.Collections.Generic;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGroupSubjectRepository : IGenericRepository<GroupSubjectEntity, int>
    {
        IEnumerable<SubjectEntity> GetSubjectsForDto(StudySearchDto searchDto);
        IEnumerable<GroupSubjectEntity> GetSubjectForGroupForDto(StudySearchDto searchDto);
        IEnumerable<StudyGroupEntity> GetStudyGroupsForDto(int? courseId);
    }
}
