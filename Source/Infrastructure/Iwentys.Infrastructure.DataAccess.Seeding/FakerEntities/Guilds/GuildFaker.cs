using Bogus;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Guilds.Models;

namespace Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Guilds
{
    public class GuildFaker
    {
        public static readonly GuildFaker Instance = new GuildFaker();

        private readonly Faker _faker = new Faker();

        public TributeCompleteRequest NewTributeCompleteRequest(long tributeId)
        {
            return new TributeCompleteRequest
            {
                Mark = _faker.Random.Int(0, 10),
                DifficultLevel = _faker.Random.Int(0, 10),
                Comment = _faker.Lorem.Paragraph(),
                TributeId = tributeId
            };
        }

        public GuildPinnedProject CreateGuildPinnedProject(int guildId)
        {
            return new GuildPinnedProject
            {
                Id = GithubRepositoryFaker.Instance.GetId(),
                GuildId = guildId
            };
        }

        public GuildCreateRequestDto GetGuildCreateArguments(GuildHiringPolicy policy = GuildHiringPolicy.Open)
        {
            return new GuildCreateRequestDto
            {
                Title = _faker.Company.CompanyName(),
                HiringPolicy = policy
            };
        }

        public Faker<Guild> CreateGuildFaker()
        {
            return new Faker<Guild>()
                .RuleFor(g => g.Id, f => f.IndexVariable++ + 1)
                .RuleFor(g => g.Title, f => f.Company.CompanyName())
                .RuleFor(g => g.Bio, f => f.Lorem.Paragraph())
                .RuleFor(g => g.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(g => g.HiringPolicy, GuildHiringPolicy.Open)
                .RuleFor(g => g.GuildType, GuildType.Created);
        }
    }
}