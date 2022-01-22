using Iwentys.AccountManagement;
using Iwentys.Domain.Raids;
using Iwentys.EntityManager.ApiClient;
using Iwentys.WebService.Application;

namespace Iwentys.Gamification;

public class RaidPartySearchRequestDto
{
    public RaidPartySearchRequestDto(RaidPartySearchRequest request) : this()
    {
        Author = EntityManagerApiDtoMapper.Map(request.Author);
        Description = request.Description;
    }

    public RaidPartySearchRequestDto()
    {
    }

    public IwentysUserInfoDto Author { get; set; }
    public string Description { get; set; }
}