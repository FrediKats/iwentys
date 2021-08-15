using Iwentys.Domain.AccountManagement.Dto;
using Iwentys.Domain.Raids.Models;

namespace Iwentys.Infrastructure.Application.Controllers.Raids.Dtos
{
    public class RaidPartySearchRequestDto
    {
        public RaidPartySearchRequestDto(RaidPartySearchRequest request) : this()
        {
            Author = new IwentysUserInfoDto(request.Author);
            Description = request.Description;
        }

        public RaidPartySearchRequestDto()
        {
        }

        public IwentysUserInfoDto Author { get; set; }
        public string Description { get; set; }
    }
}