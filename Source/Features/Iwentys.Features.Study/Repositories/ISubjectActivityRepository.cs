using System.Collections.Generic;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;

namespace Iwentys.Features.Study.Repositories
{
    public interface ISubjectActivityRepository
    {
        IReadOnlyCollection<SubjectActivity> GetStudentActivities(StudySearchParametersDto searchParametersDto);
    }
}