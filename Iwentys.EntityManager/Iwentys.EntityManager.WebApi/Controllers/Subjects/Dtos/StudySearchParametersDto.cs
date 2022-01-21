using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.PublicTypes;

namespace Iwentys.EntityManager.WebApi;

public record StudySearchParametersDto
{
    public StudySearchParametersDto(int? groupId, int? subjectId, int? courseId, StudySemester? studySemester, int skip, int take)
        : this()
    {
        GroupId = groupId;
        SubjectId = subjectId;
        CourseId = courseId;
        StudySemester = studySemester;
        Skip = skip;
        Take = take;
    }

    public StudySearchParametersDto()
    {
    }

    public int? GroupId { get; init; }
    public int? SubjectId { get; init; }
    public int? CourseId { get; init; }
    public StudySemester? StudySemester { get; init; }
    public int Skip { get; init; }
    public int Take { get; init; }

    public static StudySearchParametersDto ForGroup(int groupId)
    {
        return new StudySearchParametersDto(groupId, null, null, null, 0, 20);
    }
}