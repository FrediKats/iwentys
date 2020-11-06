using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface ISubjectActivityRepository : IGenericRepository<SubjectActivityEntity>
    {
        SubjectActivityEntity Create(SubjectActivityEntity entity);
        IReadOnlyCollection<SubjectActivityEntity> GetStudentActivities(StudySearchParameters searchParameters);
    }
}