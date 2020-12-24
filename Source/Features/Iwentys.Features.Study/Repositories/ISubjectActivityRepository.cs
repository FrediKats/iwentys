using System.Collections.Generic;
using Iwentys.Common.Databases;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;

namespace Iwentys.Features.Study.Repositories
{
    public interface ISubjectActivityRepository : IRepository<SubjectActivityEntity>
    {
        IReadOnlyCollection<SubjectActivityEntity> GetStudentActivities(StudySearchParametersDto searchParametersDto);
    }
}