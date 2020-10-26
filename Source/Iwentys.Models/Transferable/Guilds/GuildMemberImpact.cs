using Iwentys.Models.Entities.Github;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildMemberImpact
    {
        public GuildMemberImpact()
        {
        }

        public GuildMemberImpact(GithubUserEntity userEntity) : this()
        {
            Username = userEntity.Username;
            TotalRate = userEntity.ContributionFullInfo?.Total ?? 0;
        }

        public GuildMemberImpact(string username, int totalRate)
        {
            Username = username;
            TotalRate = totalRate;
        }

        public string Username { get; set; }
        public int TotalRate { get; set; }
    }
}