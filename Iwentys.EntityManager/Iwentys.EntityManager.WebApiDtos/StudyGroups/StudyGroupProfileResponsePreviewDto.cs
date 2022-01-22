namespace Iwentys.EntityManager.WebApiDtos;

public record StudyGroupProfileResponsePreviewDto
{
    public int Id { get; init; }
    public string GroupName { get; init; }
    public int? GroupAdminId { get; set; }
}