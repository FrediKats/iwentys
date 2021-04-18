using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Extended.Models
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