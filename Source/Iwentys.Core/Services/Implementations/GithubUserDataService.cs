using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Types.Github;

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

        public GithubUserData Create(int studentId, string username)
        {
            var githubUser = _githubApiAccessor.GetGithubUser(username);
            var contributionFullInfo = _githubApiAccessor.GetUserActivity(username);
            var student = _studentRepository.ReadById(studentId);

            var githubUserData = new GithubUserData
            {
                StudentId = studentId,
                Student = student,
                GithubUser = githubUser,
                ContributionFullInfo = contributionFullInfo
            };

            var studentProjects = _githubApiAccessor.GetUserRepositories(username)
                .Select(r => new StudentProject
                {
                    StudentId = studentId,
                    Student = student,
                    Url = r.Url,
                    Name = r.Name,
                    Description = r.Description,
                    StarCount = r.StarCount,
                    GithubRepositoryId = r.Id
                });

            _studentProjectRepository.CreateMany(studentProjects);

            _githubUserDataRepository.Create(githubUserData);

            return githubUserData;
        }

        public GithubUserData Update(int id)
        {
            var githubUserData = _githubUserDataRepository.ReadById(id);
            var contributionFullInfo = _githubApiAccessor.GetUserActivity(githubUserData.GithubUser.Name);
            var studentProjects = _githubApiAccessor.GetUserRepositories(githubUserData.GithubUser.Name)
                .Select(r => new StudentProject
                {
                    StudentId = githubUserData.StudentId,
                    Student = githubUserData.Student,
                    Url = r.Url,
                    Name = r.Name,
                    Description = r.Description,
                    StarCount = r.StarCount,
                    GithubRepositoryId = r.Id
                });
            foreach (var project in studentProjects)
            {
                if (_studentProjectRepository.Contains(project))
                    _studentProjectRepository.Update(project);
                else
                    _studentProjectRepository.Create(project);
            }

            githubUserData.ContributionFullInfo = contributionFullInfo;

            _githubUserDataRepository.Update(githubUserData);

            return githubUserData;
        }

        public GithubUserData GetUserDataByUsername(string username)
        {
            return _githubUserDataRepository.GetUserDataByUsername(username);
        }

        public IEnumerable<GithubRepository> GetGithubRepositories(string username)
        {
            return _studentProjectRepository.GetProjectsByUserName(username)
                .Select(p => new GithubRepository(p));
        }

        public GithubRepository GetCertainRepository(string username, string projectName)
        {
            return new GithubRepository(_studentProjectRepository.GetCertainProject(username, projectName));
        }
    }
}
