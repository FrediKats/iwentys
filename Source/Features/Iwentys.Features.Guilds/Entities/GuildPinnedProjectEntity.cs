namespace Iwentys.Features.Guilds.Entities
{
    public class GuildPinnedProjectEntity
    {
        public int Id { get; set; }

        public GuildEntity Guild { get; set; }
        public int GuildId { get; set; }

        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }
    }
}