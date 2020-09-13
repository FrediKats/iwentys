using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Tools;

namespace Iwentys.Core.Services.Implementations
{
    public class GithubUserDataService : IGithubUserDataService
    {
        private readonly IGithubUserDataRepository _githubUserDataRepository;
        private readonly IGithubApiAccessor _githubApiAccessor;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentProjectRepository _studentProjectRepository;

        public GithubUserDataService(IGithubUserDataRepository githubUserDataRepository, IGithubApiAccessor githubApiAccessor, IStudentRepository studentRepository, IStudentProjectRepository studentProjectRepository)
        {
            _githubUserDataRepository = githubUserDataRepository;
            _githubApiAccessor = githubApiAccessor;
            _studentRepository = studentRepository;
            _studentProjectRepository = studentProjectRepository;
        }

        public GithubUserData CreateOrUpdate(int studentId)
        {
            var student = _studentRepository.ReadById(studentId);
            if (student.GithubUsername == null)
                return null;
            var githubUserData = _githubUserDataRepository.Read().SingleOrDefault(gh => gh.StudentId == studentId);
            bool exists = true;

            if (githubUserData == null)
            {
                exists = false;
                var githubUser = _githubApiAccessor.GetGithubUser(student.GithubUsername);
                var contributionFullInfo = _githubApiAccessor.GetUserActivity(student.GithubUsername);

                githubUserData = new GithubUserData
                {
                    StudentId = studentId,
                    Student = student,
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
                    if (_studentProjectRepository.Contains(project))
                        _studentProjectRepository.Update(project);
                    else
                        _studentProjectRepository.Create(project);
                }

                githubUserData.ContributionFullInfo = _githubApiAccessor.GetUserActivity(student.GithubUsername);

                _githubUserDataRepository.Update(githubUserData);
            }
            else
            {
                _studentProjectRepository.CreateMany(studentProjects);

                _githubUserDataRepository.Create(githubUserData);
            }

            return githubUserData;
        }

        public GithubUserData FindByUsername(string username)
        {
            return _githubUserDataRepository.FindByUsername(username);
        }

        public IEnumerable<GithubRepository> GetGithubRepositories(string username)
        {
            return _studentProjectRepository.FindProjectsByUserName(username)
                .Select(p => new GithubRepository(p));
        }

        public GithubRepository GetCertainRepository(string username, string projectName)
        {
            return _studentProjectRepository.FindCertainProject(username, projectName).Maybe(s => new GithubRepository(s));
        }

        public IEnumerable<GithubUserData> GetAll()
        {
            return _githubUserDataRepository.Read();
        }
    }
}
