using System;

namespace Iwentys.Models.Transferable.Guilds
{
    public class MemberImpact
    {
        public String Username { get; set; }
        public Int32 TotalRate { get; set; }

        public MemberImpact(String username, Int32 totalRate)
        {
            Username = username;
            TotalRate = totalRate;
        }
    }
}