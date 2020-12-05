using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.GithubIntegration.Repositories;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.GithubIntegration.Services
{
    public class GithubIntegrationService
    {
        private readonly IGithubApiAccessor _githubApiAccessor;
        private readonly IGithubUserDataRepository _githubUserDataRepository;
        private readonly IStudentProjectRepository _studentProjectRepository;
        private readonly IStudentRepository _studentRepository;

        public GithubIntegrationService(IGithubApiAccessor githubApiAccessor, IGithubUserDataRepository githubUserDataRepository, IStudentProjectRepository studentProjectRepository, IStudentRepository studentRepository)
        {
            _githubApiAccessor = githubApiAccessor;
            _githubUserDataRepository = githubUserDataRepository;
            _studentProjectRepository = studentProjectRepository;
            _studentRepository = studentRepository;
        }

        public async Task<GithubUserEntity> CreateOrUpdate(int studentId)
        {
            StudentEntity student = await _studentRepository.GetAsync(studentId);
            GithubUserEntity githubUserData = _githubUserDataRepository.Read().SingleOrDefault(gh => gh.StudentId == studentId);
            bool exists = true;

            if (githubUserData is null)
            {
                exists = false;
                GithubUserInfoDto githubUser = _githubApiAccessor.GetGithubUser(student.GithubUsername);
                ContributionFullInfo contributionFullInfo = _githubApiAccessor.GetUserActivity(student.GithubUsername);

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

            IEnumerable<GithubProjectEntity> studentProjects = _githubApiAccessor
                .GetUserRepositories(student.GithubUsername)
                .Select(r => new GithubProjectEntity(student, r));

            if (exists)
            {
                foreach (GithubProjectEntity project in studentProjects)
                {
                    if (_studentProjectRepository.Contains(project))
                        await _studentProjectRepository.UpdateAsync(project);
                    else
                        _studentProjectRepository.Create(project);
                }

                githubUserData.ContributionFullInfo = _githubApiAccessor.GetUserActivity(student.GithubUsername);
                await _githubUserDataRepository.UpdateAsync(githubUserData);
            }
            else
            {
                _githubUserDataRepository.Create(githubUserData);
                _studentProjectRepository.CreateMany(studentProjects);
            }

            return githubUserData;
        }

        public Task<GithubUserEntity> FindByUsername(string username)
        {
            return _githubUserDataRepository.FindByUsernameAsync(username);
        }

        public IEnumerable<GithubRepositoryInfoDto> GetGithubRepositories(string username)
        {
            return _studentProjectRepository
                .FindProjectsByUserName(username)
                .Select(p => new GithubRepositoryInfoDto(p));
        }

        public GithubRepositoryInfoDto GetCertainRepository(string username, string projectName)
        {
            GithubProjectEntity githubRepository = _studentProjectRepository.FindCertainProject(username, projectName);
            if (githubRepository is null)
            {
                return _githubApiAccessor.GetRepository(username, projectName);
            }

            return new GithubRepositoryInfoDto(githubRepository);
        }

        public IEnumerable<GithubUserEntity> GetAll()
        {
            return _githubUserDataRepository.Read();
        }

        public Task<GithubUserEntity> Read(int studentId)
        {
            return _githubUserDataRepository
                .Read()
                .SingleAsync(g => g.StudentId == studentId);
        }

        public async Task<IReadOnlyList<GithubRepositoryInfoDto>> GetStudentRepositories(int studentId)
        {
            //TODO: throw exception?
            StudentEntity student = await _studentRepository.GetAsync(studentId);
            if (student.GithubUsername is null)
                return new List<GithubRepositoryInfoDto>();

            return _githubApiAccessor.GetUserRepositories(student.GithubUsername);
        }
    }
}
