using System.Collections.Generic;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ISubjectActivityRepository : IGenericRepository<SubjectActivity, int>
    {
        SubjectActivity GetActivityForStudentAndSubject(int studentId, int subjectForGroupId);
        IEnumerable<SubjectActivity> GetStudentActivities(StudySearchDto searchDto);

    }
}
