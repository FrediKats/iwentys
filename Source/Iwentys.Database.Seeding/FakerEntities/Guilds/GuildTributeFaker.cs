using Bogus;
using Iwentys.Features.Guilds.Tributes.Models;

namespace Iwentys.Database.Seeding.FakerEntities.Guilds
{
    public class GuildTributeFaker
    {
        public static readonly GuildTributeFaker Instance = new GuildTributeFaker();

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
    }
}