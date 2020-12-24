using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
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

        public async Task<GithubRepositoryInfoDto> GetRepository(string username, string projectName, bool useCache = true)
        {
            if (!useCache)
                return await _githubApiAccessor.GetRepository(username, projectName);

            GithubProjectEntity githubRepository = await _studentProjectRepository
                .GetAsync()
                .SingleOrDefaultAsync(p => p.UserName == username && p.Name == projectName);
            
            if (githubRepository is null)
                return await _githubApiAccessor.GetRepository(username, projectName);

            return new GithubRepositoryInfoDto(githubRepository);
        }

        public Task<List<GithubRepositoryInfoDto>> GetUserRepositories(string username, bool useCache = true)
        {
            if (!useCache)
                return _githubApiAccessor.GetUserRepositories(username);

            return _studentProjectRepository
                .GetAsync()
                .Where(p => p.UserName == username)
                .Select(GithubRepositoryInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<GithubUserEntity> CreateOrUpdate(int studentId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(studentId);
            GithubUserEntity githubUserData = _githubUserDataRepository.GetAsync().SingleOrDefault(gh => gh.StudentId == studentId);
            bool exists = true;

            if (githubUserData is null)
            {
                exists = false;
                GithubUserInfoDto githubUser = await _githubApiAccessor.GetGithubUser(student.GithubUsername);
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

            IEnumerable<GithubProjectEntity> studentProjects = (await _githubApiAccessor
                    .GetUserRepositories(student.GithubUsername))
                .Select(r => new GithubProjectEntity(student, r));

            if (exists)
            {
                foreach (GithubProjectEntity project in studentProjects)
                {
                    if (_studentProjectRepository.GetByIdAsync(project.Id) is null)
                        _studentProjectRepository.Update(project);
                    else
                    {
                        await _studentProjectRepository.InsertAsync(project);
                    }
                }

                githubUserData.ContributionFullInfo = _githubApiAccessor.GetUserActivity(student.GithubUsername);
                _githubUserDataRepository.Update(githubUserData);
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

        public Task<GithubUserEntity> GetGithubUser(string username)
        {
            return _githubUserDataRepository.GetAsync().SingleOrDefaultAsync(g => g.Username == username);
        }
        
        public Task<List<GithubUserEntity>> GetAll()
        {
            return _githubUserDataRepository
                .GetAsync()
                .ToListAsync();
        }

        public Task<GithubUserEntity> Read(int studentId)
        {
            return _githubUserDataRepository
                .GetAsync()
                .SingleAsync(g => g.StudentId == studentId);
        }

        public async Task<IReadOnlyList<GithubRepositoryInfoDto>> GetStudentRepositories(int studentId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(studentId);
            if (student.GithubUsername is null)
                throw new InnerLogicException("Student do not link github");

            return await _githubApiAccessor.GetUserRepositories(student.GithubUsername);
        }
    }
}
