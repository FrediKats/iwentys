using System.Linq.Expressions;

namespace Iwentys.EntityManager.WebApiDtos;

public record GroupProfileResponsePreviewDto
{
    public int Id { get; init; }
    public string GroupName { get; init; }
    public int? GroupAdminId { get; set; }

    public GroupProfileResponsePreviewDto()
    {
    }
}