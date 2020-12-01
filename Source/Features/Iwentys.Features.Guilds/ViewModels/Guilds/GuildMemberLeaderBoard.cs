using System.Collections.Generic;
using Iwentys.Features.StudentFeature.ViewModels;

namespace Iwentys.Features.Guilds.ViewModels.Guilds
{
    public class GuildMemberLeaderBoard
    {
        public int TotalRate { get; set; }
        public List<StudentPartialProfileDto> Members { get; set; }
        public List<GuildMemberImpact> MembersImpact { get; set; }
    }
}