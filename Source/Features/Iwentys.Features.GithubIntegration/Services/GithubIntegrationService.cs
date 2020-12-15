using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.GithubIntegration.Services
{
    public class GithubIntegrationService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<GithubProjectEntity> _studentProjectRepository;
        private readonly IGenericRepository<GithubUserEntity> _githubUserDataRepository;

        private readonly IGithubApiAccessor _githubApiAccessor;

        public GithubIntegrationService(IGithubApiAccessor githubApiAccessor, IUnitOfWork unitOfWork)
        {
            _githubApiAccessor = githubApiAccessor;
            
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _studentProjectRepository = _unitOfWork.GetRepository<GithubProjectEntity>();
            _githubUserDataRepository = _unitOfWork.GetRepository<GithubUserEntity>();
        }

        public async Task<GithubUserEntity> CreateOrUpdate(int studentId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(studentId);
            GithubUserEntity githubUserData = _githubUserDataRepository.GetAsync().SingleOrDefault(gh => gh.StudentId == studentId);
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
                    if (_studentProjectRepository.GetByIdAsync(project.Id) is null)
                        await _studentProjectRepository.UpdateAsync(project);
                    else
                    {
                        await _studentProjectRepository.InsertAsync(project);
                    }
                }

                githubUserData.ContributionFullInfo = _githubApiAccessor.GetUserActivity(student.GithubUsername);
                await _githubUserDataRepository.UpdateAsync(githubUserData);
            }
            else
            {
                await _githubUserDataRepository.InsertAsync(githubUserData);
                foreach (var githubProjectEntity in studentProjects)
                {
                    await _studentProjectRepository.InsertAsync(githubProjectEntity);
                }
            }

            await _unitOfWork.CommitAsync();

            return githubUserData;
        }

        public Task<GithubUserEntity> FindByUsername(string username)
        {
            return _githubUserDataRepository.GetAsync().SingleOrDefaultAsync(g => g.Username == username);
        }

        public IEnumerable<GithubRepositoryInfoDto> GetGithubRepositories(string username)
        {
            return _studentProjectRepository
                .GetAsync()
                .Where(p => p.UserName == username)
                .Select(p => new GithubRepositoryInfoDto(p));
        }

        public GithubRepositoryInfoDto GetCertainRepository(string username, string projectName)
        {
            GithubProjectEntity githubRepository = _studentProjectRepository
                    .GetAsync()
                .SingleOrDefault(p => p.UserName == username && p.Name == projectName);
            if (githubRepository is null)
            {
                return _githubApiAccessor.GetRepository(username, projectName);
            }

            return new GithubRepositoryInfoDto(githubRepository);
        }

        public IEnumerable<GithubUserEntity> GetAll()
        {
            return _githubUserDataRepository.GetAsync().ToList();
        }

        public Task<GithubUserEntity> Read(int studentId)
        {
            return _githubUserDataRepository
                .GetAsync()
                .SingleAsync(g => g.StudentId == studentId);
        }

        public async Task<IReadOnlyList<GithubRepositoryInfoDto>> GetStudentRepositories(int studentId)
        {
            //TODO: throw exception?
            StudentEntity student = await _studentRepository.GetByIdAsync(studentId);
            if (student.GithubUsername is null)
                return new List<GithubRepositoryInfoDto>();

            return _githubApiAccessor.GetUserRepositories(student.GithubUsername);
        }
    }
}
