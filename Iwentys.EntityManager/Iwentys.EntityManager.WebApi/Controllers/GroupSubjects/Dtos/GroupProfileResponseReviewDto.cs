using System.Linq.Expressions;
using Iwentys.EntityManager.Domain;

namespace Iwentys.EntityManager.WebApi;

public record GroupProfileResponsePreviewDto
{
    public int Id { get; init; }
    public string GroupName { get; init; }
    public int? GroupAdminId { get; set; }

    public GroupProfileResponsePreviewDto()
    {
    }

    public GroupProfileResponsePreviewDto(StudyGroup entity)
    {
        Id = entity.Id;
        GroupName = entity.GroupName;
        GroupAdminId = entity.GroupAdminId;
    }

    public static Expression<Func<StudyGroup, GroupProfileResponsePreviewDto>> FromEntity =>
        entity => new GroupProfileResponsePreviewDto
        {
            Id = entity.Id,
            GroupName = entity.GroupName,
            GroupAdminId = entity.GroupAdminId
        };
}