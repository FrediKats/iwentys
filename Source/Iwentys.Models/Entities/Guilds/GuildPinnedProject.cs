namespace Iwentys.Models.Entities.Guilds
{
    public class GuildPinnedProject
    {
        public int Id { get; set; }

        public Guild Guild { get; set; }
        public int GuildId { get; set; }

        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }
    }
}