using Bogus;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class GuildPinnedProjectFaker
    {
        private readonly Faker _faker = new Faker();

        public GuildPinnedProjectFaker()
        {
        }

        public GuildPinnedProject CreatePinnedProject(GithubProject project, int? id)
        {
            return new GuildPinnedProject()
            {
                Id = id ?? _faker.IndexFaker + 1,
            };
        }
    }
}