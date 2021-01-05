using Bogus;
using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class GithubRepositoryFaker
    {
        public static readonly GithubRepositoryFaker Instance = new GithubRepositoryFaker();

        private readonly Faker _faker = new Faker();

        public GithubRepositoryInfoDto Generate(string githubUsername)
        {
            return new GithubRepositoryInfoDto(
                _faker.IndexVariable++,
                githubUsername,
                _faker.Company.CompanyName(),
                _faker.Lorem.Paragraph(),
                _faker.Internet.Url(),
                _faker.Random.Int(0, 100));
        }
    }
}