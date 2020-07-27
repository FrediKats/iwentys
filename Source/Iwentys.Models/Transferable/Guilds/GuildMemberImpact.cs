using System;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildMemberImpact
    {
        public String Username { get; set; }
        public Int32 TotalRate { get; set; }

        public GuildMemberImpact(String username, Int32 totalRate)
        {
            Username = username;
            TotalRate = totalRate;
        }
    }
}