namespace Iwentys.EntityManager.WebApiDtos;

public class GroupTeachersDto
{
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public IReadOnlyList<TeacherDto> Teachers { get; set; }

    public GroupTeachersDto()
    {
    }

    public GroupTeachersDto(int groupId, string name, IReadOnlyList<TeacherDto> teachers)
    {
        GroupId = groupId;
        GroupName = name;
        Teachers = teachers;
    }
}