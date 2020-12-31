using Bogus;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class GuildPinnedProjectFaker : Faker<GuildPinnedProject>
    {
        public GuildPinnedProjectFaker()
        {
            this
                .RuleFor(g => g.Id, f => f.IndexFaker + 1)
                .RuleFor(gp => gp.RepositoryOwner, f => f.Company.CompanyName())
                .RuleFor(gp => gp.RepositoryName, f => f.Company.CompanyName());
        }
    }
}