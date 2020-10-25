using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Models;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Tools;

namespace Iwentys.Core.Services.Implementations
{
    public class GithubUserDataService : IGithubUserDataService
    {
        private readonly IGithubApiAccessor _githubApiAccessor;
        private readonly DatabaseAccessor _database;

        public GithubUserDataService(DatabaseAccessor database, IGithubApiAccessor githubApiAccessor)
        {
            _githubApiAccessor = githubApiAccessor;
            _database = database;
        }

        public GithubUserEntity CreateOrUpdate(int studentId)
        {
            var student = _database.Student.ReadById(studentId);
            if (student.GithubUsername == null)
                return null;
            var githubUserData = _database.GithubUserData.Read().SingleOrDefault(gh => gh.StudentId == studentId);
            bool exists = true;

            if (githubUserData == null)
            {
                exists = false;
                var githubUser = _githubApiAccessor.GetGithubUser(student.GithubUsername);
                var contributionFullInfo = _githubApiAccessor.GetUserActivity(student.GithubUsername);

                githubUserData = new GithubUserEntity
                {
                    StudentId = studentId,
                    Username = student.GithubUsername,
                    AvatarUrl = githubUser.AvatarUrl,
                    Bio = githubUser.Bio,
                    Company = githubUser.Bio,
                    ContributionFullInfo = contributionFullInfo
                };
            }

            var studentProjects = _githubApiAccessor
                .GetUserRepositories(student.GithubUsername)
                .Select(r => new GithubProjectEntity(student, r));

            if (exists)
            {
                foreach (var project in studentProjects)
                {
                    if (_database.StudentProject.Contains(project))
                        _database.StudentProject.Update(project);
                    else
                        _database.StudentProject.Create(project);
                }

                githubUserData.ContributionFullInfo = _githubApiAccessor.GetUserActivity(student.GithubUsername);

                _database.GithubUserData.Update(githubUserData);
            }
            else
            {
                _database.StudentProject.CreateMany(studentProjects);

                _database.GithubUserData.Create(githubUserData);
            }

            return githubUserData;
        }

        public GithubUserEntity FindByUsername(string username)
        {
            return _database.GithubUserData.FindByUsername(username);
        }

        public IEnumerable<GithubRepository> GetGithubRepositories(string username)
        {
            return _database.StudentProject.FindProjectsByUserName(username)
                .Select(p => new GithubRepository(p));
        }

        public GithubRepository GetCertainRepository(string username, string projectName)
        {
            return _database.StudentProject.FindCertainProject(username, projectName).Maybe(s => new GithubRepository(s));
        }

        public IEnumerable<GithubUserEntity> GetAll()
        {
            return _database.GithubUserData.Read();
        }
    }
}
