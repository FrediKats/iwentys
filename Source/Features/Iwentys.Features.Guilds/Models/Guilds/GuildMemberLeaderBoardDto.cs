using System.Collections.Generic;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public record GuildMemberLeaderBoardDto(int TotalRate, List<StudentInfoDto> Members, List<GuildMemberImpactDto> MembersImpact)
    {
    }
}