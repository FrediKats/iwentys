using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Guilds
{
    public record GuildMemberLeaderBoardDto
    {
        public GuildMemberLeaderBoardDto(List<GuildMemberImpactDto> membersImpact) : this()
        {
            TotalRate = membersImpact.Sum(m => m.TotalRate);
            MembersImpact = membersImpact;
        }

        public GuildMemberLeaderBoardDto()
        {
        }

        public List<GuildMemberImpactDto> MembersImpact { get; init; }
        public int TotalRate { get; init; }
    }
}