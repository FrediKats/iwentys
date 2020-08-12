using System.Collections.Generic;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ISubjectForGroupRepository : IGenericRepository<SubjectForGroup, int>
    {
        IEnumerable<Subject> GetSubjectsForDto(StudySearchDto searchDto);
        IEnumerable<SubjectForGroup> GetSubjectForGroupForDto(StudySearchDto searchDto);
        IEnumerable<StudyGroup> GetStudyGroupsForDto(StudySearchDto searchDto);
    }
}
