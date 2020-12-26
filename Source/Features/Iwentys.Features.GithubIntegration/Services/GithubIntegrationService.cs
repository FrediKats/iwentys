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
        private readonly IGenericRepository<GithubUserEntity> _githubUserRepository;

        private readonly IGithubApiAccessor _githubApiAccessor;

        public GithubIntegrationService(IGithubApiAccessor githubApiAccessor, IUnitOfWork unitOfWork)
        {
            _githubApiAccessor = githubApiAccessor;
            
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _studentProjectRepository = _unitOfWork.GetRepository<GithubProjectEntity>();
            _githubUserRepository = _unitOfWork.GetRepository<GithubUserEntity>();
        }

        public async Task<GithubRepositoryInfoDto> GetRepository(string username, string projectName, bool useCache = true)
        {
            if (!useCache)
            {
                StudentEntity student = await EnsureStudentWithGithub(username);
                await ForceRescanUserRepositories(student);
            }

            GithubProjectEntity githubRepository = await _studentProjectRepository
                .GetAsync()
                .SingleOrDefaultAsync(p => p.UserName == username && p.Name == projectName);
            
            if (githubRepository is null)
                return await _githubApiAccessor.GetRepository(username, projectName);

            return new GithubRepositoryInfoDto(githubRepository);
        }

        public async Task<List<GithubRepositoryInfoDto>> GetStudentRepositories(int studentId)
        {
            StudentEntity student = await EnsureStudentWithGithub(studentId);

            return await GetUserRepositories(student.GithubUsername);
        }
        
        public async Task<List<GithubRepositoryInfoDto>> GetUserRepositories(string username, bool useCache = true)
        {
            if (!useCache)
            {
                StudentEntity student = await EnsureStudentWithGithub(username);
                await ForceRescanUserRepositories(student);
            }

            return await _studentProjectRepository
                .GetAsync()
                .Where(p => p.UserName == username)
                .Select(GithubRepositoryInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<GithubUserEntity> ForceRescanUser(StudentEntity student, GithubUserEntity oldGithubUser)
        {
            GithubUserInfoDto githubUser = await _githubApiAccessor.GetGithubUser(student.GithubUsername);
            ContributionFullInfo contributionFullInfo = await _githubApiAccessor.GetUserActivity(student.GithubUsername);

            if (oldGithubUser is null)
            {
                var githubUserEntity = GithubUserEntity.Create(student, githubUser, contributionFullInfo);
                await _githubUserRepository.InsertAsync(githubUserEntity);
                return githubUserEntity;
            }
            else
            {
                oldGithubUser.Update(githubUser, contributionFullInfo);
                _githubUserRepository.Update(oldGithubUser);
                await _unitOfWork.CommitAsync();
                return oldGithubUser;
            }
        }

        //TODO: rework but not now. Probably perf problems
        //TODO: remove old repo that is not exist in githubRepositories
        public async Task ForceRescanUserRepositories(StudentEntity student)
        {
            List<GithubRepositoryInfoDto> githubRepositories = await _githubApiAccessor.GetUserRepositories(student.GithubUsername);
            IEnumerable<GithubProjectEntity> studentProjects = githubRepositories.Select(r => new GithubProjectEntity(student, r));
            foreach (GithubProjectEntity project in studentProjects)
            {
                if (_studentProjectRepository.GetByIdAsync(project.Id) is null)
                    _studentProjectRepository.Update(project);
                else
                {
                    await _studentProjectRepository.InsertAsync(project);
                }
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<GithubUserEntity> CreateOrUpdate(int studentId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(studentId);
            GithubUserEntity githubUserData = _githubUserRepository.GetAsync().SingleOrDefault(gh => gh.StudentId == studentId);
            bool exists = true;

            if (githubUserData is null)
            {
                exists = false;
                GithubUserInfoDto githubUser = await _githubApiAccessor.GetGithubUser(student.GithubUsername);
                ContributionFullInfo contributionFullInfo = await _githubApiAccessor.GetUserActivity(student.GithubUsername);

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

                githubUserData.ContributionFullInfo = await _githubApiAccessor.GetUserActivity(student.GithubUsername);
                _githubUserRepository.Update(githubUserData);
            }
            else
            {
                await _githubUserRepository.InsertAsync(githubUserData);
                foreach (var githubProjectEntity in studentProjects)
                {
                    await _studentProjectRepository.InsertAsync(githubProjectEntity);
                }
            }

            await _unitOfWork.CommitAsync();

            return githubUserData;
        }

        public Task<List<GithubUserEntity>> GetAllGithubUser()
        {
            return _githubUserRepository
                .GetAsync()
                .ToListAsync();
        }

        public async Task<GithubUserEntity> GetGithubUser(string username, bool useCache = true)
        {
            var result = await _githubUserRepository
                .GetAsync()
                .SingleOrDefaultAsync(g => g.Username == username);

            if (!useCache)
            {
                var student = await EnsureStudentWithGithub(username);
                result = await ForceRescanUser(student, result);
            }

            return result;
        }

        //TODO: why this is Find? Do we need this?
        public async Task<GithubUserEntity> FindGithubUser(int studentId, bool useCache = true)
        {
            var result = await _githubUserRepository
                .GetAsync()
                .SingleOrDefaultAsync(g => g.StudentId == studentId);

            //TODO: if null we... can try get from api?
            if (!useCache && result is not null)
            {
                var student = await EnsureStudentWithGithub(studentId);
                result = await ForceRescanUser(student, result);
            }

            return result;
        }

        //TODO: wrap with domain entity?
        private async Task<StudentEntity> EnsureStudentWithGithub(int studentId)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(studentId);
            if (student.GithubUsername is null)
                throw new InnerLogicException("Student do not link github");
            return student;
        }

        private async Task<StudentEntity> EnsureStudentWithGithub(string githubUsername)
        {
            StudentEntity student = await _studentRepository.GetAsync().SingleAsync(s => s.GithubUsername == githubUsername);
            if (student.GithubUsername is null)
                throw new InnerLogicException("Student do not link github");
            return student;
        }
    }
}
