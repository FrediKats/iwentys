using System.Collections.Generic;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGroupSubjectRepository : IGenericRepository<GroupSubjectEntity, int>
    {
        IEnumerable<SubjectEntity> GetSubjectsForDto(StudySearchParameters searchParameters);
        IEnumerable<GroupSubjectEntity> GetSubjectForGroupForDto(StudySearchParameters searchParameters);
        IEnumerable<StudyGroupEntity> GetStudyGroupsForDto(int? courseId);
    }
}