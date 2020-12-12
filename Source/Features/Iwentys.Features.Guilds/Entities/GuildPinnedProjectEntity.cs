namespace Iwentys.Features.Guilds.Entities
{
    public class GuildPinnedProjectEntity
    {
        //FYI: It's always must be github repo id
        public long Id { get; set; }

        public virtual GuildEntity Guild { get; set; }
        public int GuildId { get; set; }

        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }
    }
}