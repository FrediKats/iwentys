using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.ViewModels;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface ISubjectActivityRepository : IGenericRepository<SubjectActivityEntity>
    {
        SubjectActivityEntity Create(SubjectActivityEntity entity);
        IReadOnlyCollection<SubjectActivityEntity> GetStudentActivities(StudySearchParameters searchParameters);
    }
}