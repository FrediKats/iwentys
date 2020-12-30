using System.Collections.Generic;
using Bogus;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class GithubDataGenerator
    {
        public List<GithubUser> GithubUserEntities { get; set; }
        public List<GithubProject> GithubProjectEntities { get; set; }

        public GithubDataGenerator(List<Student> students)
        {
            var faker = new Faker();
            faker.IndexVariable++;

            GithubUserEntities = new List<GithubUser>();
            GithubProjectEntities = new List<GithubProject>();
            foreach (Student student in students)
            {
                ActivityInfo activity = CreateActivity(faker);
                GithubUserEntities.Add(new GithubUser
                {
                    StudentId = student.Id,
                    Username = student.GithubUsername,
                    ContributionFullInfo = new ContributionFullInfo { RawActivity = activity}
                });
                
                var repositoryInfo = new GithubRepositoryInfoDto(
                    faker.IndexVariable++,
                    student.GithubUsername,
                    faker.Company.CompanyName(),
                    faker.Lorem.Paragraph(),
                    faker.Internet.Url(),
                    faker.Random.Int(0, 100));
                
                GithubProjectEntities.Add(new GithubProject(student, repositoryInfo));
            }
        }

        private static ActivityInfo CreateActivity(Faker faker)
        {
            List<ContributionsInfo> result = new List<ContributionsInfo>();
            for (int i = 1; i <= 11; i++)
            {
                result.Add(new ContributionsInfo(
                    $"2020-{RandomExtensions.Instance.Next(12) + 1:D2}-20",
                    RandomExtensions.Instance.Next(100)));
            }

            return new ActivityInfo
            {
                Contributions = result,
            };
        }
    }
}
