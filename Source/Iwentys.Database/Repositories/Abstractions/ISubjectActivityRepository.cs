using System.Collections.Generic;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ISubjectActivityRepository : IGenericRepository<SubjectActivityEntity, int>
    {
        SubjectActivityEntity Create(SubjectActivityEntity subjectActivity);

        SubjectActivityEntity GetActivityForStudentAndSubject(int studentId, int subjectForGroupId);
        IReadOnlyCollection<SubjectActivityEntity> GetStudentActivities(StudySearchParameters searchParameters);
    }
}