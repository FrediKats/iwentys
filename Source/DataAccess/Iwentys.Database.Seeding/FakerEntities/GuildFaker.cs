using Bogus;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class GuildFaker : Faker<Guild>
    {
        public GuildFaker()
        {
            this
                .RuleFor(g => g.Id, f => f.IndexVariable++ + 1)
                .RuleFor(g => g.Title, f => f.Company.CompanyName())
                .RuleFor(g => g.Bio, f => f.Lorem.Paragraph())
                .RuleFor(g => g.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(g => g.HiringPolicy, GuildHiringPolicy.Open)
                .RuleFor(g => g.GuildType, GuildType.Created);
        }
    }
}