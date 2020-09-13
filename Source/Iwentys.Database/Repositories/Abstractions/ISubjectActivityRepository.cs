using System.Collections.Generic;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ISubjectActivityRepository : IGenericRepository<SubjectActivityEntity, int>
    {
        SubjectActivityEntity Create(SubjectActivityEntity subjectActivity);

        SubjectActivityEntity GetActivityForStudentAndSubject(int studentId, int subjectForGroupId);
        IEnumerable<SubjectActivityEntity> GetStudentActivities(StudySearchDto searchDto);
    }
}
