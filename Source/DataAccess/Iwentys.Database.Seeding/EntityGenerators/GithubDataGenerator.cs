using System.Collections.Generic;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class GithubDataGenerator : IEntityGenerator
    {
        public GithubDataGenerator(List<Student> students)
        {
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

                GithubRepositoryInfoDto repositoryInfo = GithubRepositoryFaker.Instance.Generate(student.GithubUsername);

                GithubUserEntities.Add(githubUser);
                GithubProjectEntities.Add(new GithubProject(githubUser, repositoryInfo));
            }
        }

        public List<GithubUser> GithubUserEntities { get; set; }
        public List<GithubProject> GithubProjectEntities { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GithubUser>().HasData(GithubUserEntities);
            modelBuilder.Entity<GithubProject>().HasData(GithubProjectEntities);
        }

        private static ActivityInfo CreateActivity()
        {
            var result = new List<ContributionsInfo>();

            for (var i = 1; i <= 11; i++)
                result.Add(new ContributionsInfo(
                    $"2020-{RandomExtensions.Instance.Random.Int(1, 12):D2}-20",
                    RandomExtensions.Instance.Random.Int(0, 100)));

            return new ActivityInfo
            {
                Contributions = result
            };
        }
    }
}