using Iwentys.EntityManager.PublicTypes;

namespace Iwentys.EntityManager.WebApiDtos;

public record SubjectSearchParametersDto
{
    public SubjectSearchParametersDto(int? groupId, int? subjectId, int? courseId, StudySemester? studySemester, int skip, int take)
        : this()
    {
        GroupId = groupId;
        SubjectId = subjectId;
        CourseId = courseId;
        StudySemester = studySemester;
        Skip = skip;
        Take = take;
    }

    public SubjectSearchParametersDto()
    {
    }

    public int? GroupId { get; init; }
    public int? SubjectId { get; init; }
    public int? CourseId { get; init; }
    public StudySemester? StudySemester { get; init; }
    public int Skip { get; init; }
    public int Take { get; init; }

    public static SubjectSearchParametersDto ForGroup(int groupId)
    {
        return new SubjectSearchParametersDto(groupId, null, null, null, 0, 20);
    }
}