using System.Collections.Generic;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Tools;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface ISubjectActivityRepository : IGenericRepository<SubjectActivityEntity>
    {
        SubjectActivityEntity Create(SubjectActivityEntity entity);
        IReadOnlyCollection<SubjectActivityEntity> GetStudentActivities(StudySearchParameters searchParameters);
    }
}