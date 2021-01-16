using Bogus;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class GuildPinnedProjectFaker
    {
        private readonly Faker _faker = new Faker();

        public GuildPinnedProjectFaker()
        {
        }

        public GuildPinnedProject CreatePinnedProject(int guildId)
        {
            return new GuildPinnedProject()
            {
                Id = GithubRepositoryFaker.Instance.GetId(),
                GuildId = guildId
            };
        }
    }
}