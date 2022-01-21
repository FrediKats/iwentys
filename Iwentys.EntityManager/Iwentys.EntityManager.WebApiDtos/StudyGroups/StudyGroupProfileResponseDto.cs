namespace Iwentys.EntityManager.WebApiDtos;

public record StudyGroupProfileResponseDto
{
    public int Id { get; init; }
    public string GroupName { get; init; }
    public int? GroupAdminId { get; set; }
    public List<StudentInfoDto> Students { get; set; }
    public List<SubjectProfileDto> Subjects { get; init; }

    public StudentInfoDto GroupAdmin => GroupAdminId is null ? null : Students.Find(s => s.Id == GroupAdminId);
}