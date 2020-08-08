using System.Linq;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface ISubjectActivityRepository : IGenericRepository<SubjectActivity, int>
    {
        public SubjectActivity GetActivityForStudentAndSubject(int studentId, int subjectForGroupId);
    }
}
