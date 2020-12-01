using System.Collections.Generic;
using Bogus;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Integrations.GithubIntegration.Models;
using Iwentys.Models;
using Iwentys.Models.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class GithubActivityGenerator
    {
        public List<GithubUserEntity> GithubUserEntities { get; set; }

        public GithubActivityGenerator(List<StudentEntity> students)
        {
            var faker = new Faker();
            faker.IndexVariable++;

            GithubUserEntities = new List<GithubUserEntity>();
            foreach (StudentEntity student in students)
            {
                ActivityInfo activity = CreateActivity(faker);
                GithubUserEntities.Add(new GithubUserEntity
                {
                    StudentId = student.Id,
                    Username = student.GithubUsername,
                    ContributionFullInfo = new ContributionFullInfo { RawActivity = activity}
                });
            }
        }

        private ActivityInfo CreateActivity(Faker faker)
        {
            List<ContributionsInfo> result = new List<ContributionsInfo>();
            for (int i = 1; i <= 11; i++)
            {
                result.Add(new ContributionsInfo(
                    $"2020-{RandomExtensions.Instance.Next(12) + 1:D2}-20",
                    RandomExtensions.Instance.Next(100))
                    {
                        Id = faker.IndexVariable++,
                    });
            }

            return new ActivityInfo
            {
                Contributions = result,
                Id = faker.IndexVariable++,
            };
        }
    }
}
