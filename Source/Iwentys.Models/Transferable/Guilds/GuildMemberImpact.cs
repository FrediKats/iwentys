using System;
using Iwentys.Models.Entities;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildMemberImpact
    {
        public String Username { get; set; }
        public Int32 TotalRate { get; set; }

        public GuildMemberImpact()
        {
        }

        public GuildMemberImpact(GithubUserData userData) :this()
        {
            Username = userData.Username;
            TotalRate = userData.ContributionFullInfo?.Total ?? 0;
        }

        public GuildMemberImpact(String username, Int32 totalRate)
        {
            Username = username;
            TotalRate = totalRate;
        }
    }
}