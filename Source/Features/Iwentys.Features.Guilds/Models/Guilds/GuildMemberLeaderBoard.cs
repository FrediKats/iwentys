using System.Collections.Generic;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public class GuildMemberLeaderBoard
    {
        public int TotalRate { get; set; }
        public List<StudentInfoDto> Members { get; set; }
        public List<GuildMemberImpact> MembersImpact { get; set; }
    }
}