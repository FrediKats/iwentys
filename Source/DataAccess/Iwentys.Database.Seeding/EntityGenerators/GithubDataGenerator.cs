using System.Collections.Generic;
using Bogus;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class GithubDataGenerator : IEntityGenerator
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
                ActivityInfo activity = CreateActivity();
                var githubUser = new GithubUser
                {
                    IwentysUserId = student.Id,
                    Username = student.GithubUsername,
                    ContributionFullInfo = new ContributionFullInfo {RawActivity = activity}
                };

                GithubUserEntities.Add(githubUser);
                
                var repositoryInfo = new GithubRepositoryInfoDto(
                    faker.IndexVariable++,
                    student.GithubUsername,
                    faker.Company.CompanyName(),
                    faker.Lorem.Paragraph(),
                    faker.Internet.Url(),
                    faker.Random.Int(0, 100));
                
                GithubProjectEntities.Add(new GithubProject(githubUser, repositoryInfo));
            }
        }

        private static ActivityInfo CreateActivity()
        {
            List<ContributionsInfo> result = new List<ContributionsInfo>();
            for (int i = 1; i <= 11; i++)
            {
                result.Add(new ContributionsInfo(
                    $"2020-{RandomExtensions.Instance.Random.Int(1, 12):D2}-20",
                    RandomExtensions.Instance.Random.Int(0, 100)));
            }

            return new ActivityInfo
            {
                Contributions = result,
            };
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GithubUser>().HasData(GithubUserEntities);
            modelBuilder.Entity<GithubProject>().HasData(GithubProjectEntities);
        }
    }
}
