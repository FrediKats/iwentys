using System.Collections.Generic;
using Iwentys.Models.Entities;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildMemberLeaderBoard
    {
        public int TotalRate { get; set; }
        public List<Student> Members { get; set; }
        public List<GuildMemberImpact> MembersImpact { get; set; }
    }
}