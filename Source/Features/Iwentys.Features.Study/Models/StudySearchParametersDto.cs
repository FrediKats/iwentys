using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Study.Models
{
    public record StudySearchParametersDto(
        int? GroupId,
        int? SubjectId,
        int? CourseId,
        StudySemester? StudySemester,
        int Skip,
        int Take)
    {

        public static StudySearchParametersDto ForGroup(int groupId)
        {
            return new StudySearchParametersDto(groupId, null, null, null, 0, 20);
        }
    }
}