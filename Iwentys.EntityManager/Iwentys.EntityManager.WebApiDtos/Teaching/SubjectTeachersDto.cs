namespace Iwentys.EntityManager.WebApiDtos;

public record SubjectTeachersDto
{
    public int SubjectId { get; set; }
    public string Name { get; set; }
    public IReadOnlyList<GroupTeachersDto> GroupTeachers { get; set; }

    public SubjectTeachersDto()
    {

    }

    public SubjectTeachersDto(int subjectId, string name, IReadOnlyList<GroupTeachersDto> groupTeachers)
    {
        SubjectId = subjectId;
        Name = name;
        GroupTeachers = groupTeachers;
    }
}