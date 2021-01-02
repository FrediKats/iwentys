using Iwentys.Features.Raids.Entities;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Raids.Models
{
    public class RaidPartySearchRequestDto
    {
        public StudentInfoDto Author { get; set; }
        public string Description { get; set; }

        public RaidPartySearchRequestDto(RaidPartySearchRequest request) : this()
        {
            Author = new StudentInfoDto(request.Author);
            Description = request.Description;
        }

        public RaidPartySearchRequestDto()
        {
            
        }
    }
}