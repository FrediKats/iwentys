using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;

namespace Iwentys.Features.Study.Repositories
{
    public interface ISubjectActivityRepository : IGenericRepository<SubjectActivityEntity>
    {
        SubjectActivityEntity Create(SubjectActivityEntity entity);
        IReadOnlyCollection<SubjectActivityEntity> GetStudentActivities(StudySearchParametersDto searchParametersDto);
    }
}