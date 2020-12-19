using System.Collections.Generic;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public record GuildMemberLeaderBoardDto
    {
        public GuildMemberLeaderBoardDto(int totalRate, List<StudentInfoDto> members, List<GuildMemberImpactDto> membersImpact)
        {
            TotalRate = totalRate;
            Members = members;
            MembersImpact = membersImpact;
        }

        public GuildMemberLeaderBoardDto()
        {
        }
        
        public int TotalRate { get; init; }
        public List<StudentInfoDto> Members { get; init; }
        public List<GuildMemberImpactDto> MembersImpact { get; init; }
    }
}