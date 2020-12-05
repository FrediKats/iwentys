using System.Collections.Generic;
using Iwentys.Features.StudentFeature.Models;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public class GuildMemberLeaderBoard
    {
        public int TotalRate { get; set; }
        public List<StudentPartialProfileDto> Members { get; set; }
        public List<GuildMemberImpact> MembersImpact { get; set; }
    }
}