using System.Collections.Generic;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildMemberLeaderBoard
    {
        public int TotalRate { get; set; }
        public List<StudentPartialProfileDto> Members { get; set; }
        public List<GuildMemberImpact> MembersImpact { get; set; }
    }
}