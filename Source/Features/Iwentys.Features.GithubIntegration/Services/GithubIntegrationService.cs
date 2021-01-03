using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.GithubIntegration.Services
{
    public class GithubIntegrationService
    {
        private readonly IGithubApiAccessor _githubApiAccessor;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<IwentysUser> _userRepository;
        private readonly IGenericRepository<GithubProject> _studentProjectRepository;
        private readonly IGenericRepository<GithubUser> _githubUserRepository;

        public GithubIntegrationService(IGithubApiAccessor githubApiAccessor, IUnitOfWork unitOfWork)
        {
            _githubApiAccessor = githubApiAccessor;

            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
            _studentProjectRepository = _unitOfWork.GetRepository<GithubProject>();
            _githubUserRepository = _unitOfWork.GetRepository<GithubUser>();
        }

        public async Task<GithubRepositoryInfoDto> GetRepository(string username, string projectName, bool useCache = true)
        {
            if (!useCache)
            {
                IwentysUser student = await EnsureStudentWithGithub(username);
                await ForceRescanUserRepositories(student);
            }

            GithubProject githubRepository = await _studentProjectRepository
                .Get()
                .SingleOrDefaultAsync(p => p.Owner == username && p.Name == projectName);

            if (githubRepository is null)
                return await _githubApiAccessor.GetRepository(username, projectName);

            return new GithubRepositoryInfoDto(githubRepository);
        }

        public async Task<List<GithubRepositoryInfoDto>> GetStudentRepositories(int studentId)
        {
            IwentysUser student = await EnsureStudentWithGithub(studentId);

            return await GetUserRepositories(student.GithubUsername);
        }

        public async Task<List<GithubRepositoryInfoDto>> GetUserRepositories(string username, bool useCache = true)
        {
            if (!useCache)
            {
                IwentysUser student = await EnsureStudentWithGithub(username);
                await ForceRescanUserRepositories(student);
            }

            return await _studentProjectRepository
                .Get()
                .Where(p => p.Owner == username)
                .Select(GithubRepositoryInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<GithubUser> ForceRescanUser(IwentysUser student, GithubUser oldGithubUser)
        {
            GithubUserInfoDto githubUser = await _githubApiAccessor.GetGithubUser(student.GithubUsername);
            ContributionFullInfo contributionFullInfo = await _githubApiAccessor.GetUserActivity(student.GithubUsername);

            if (oldGithubUser is null)
            {
                var githubUserEntity = GithubUser.Create(student, githubUser, contributionFullInfo);
                await _githubUserRepository.InsertAsync(githubUserEntity);
                return githubUserEntity;
            }

            oldGithubUser.Update(githubUser, contributionFullInfo);
            _githubUserRepository.Update(oldGithubUser);
            await _unitOfWork.CommitAsync();
            return oldGithubUser;
        }

        //TODO: rework but not now. Probably perf problems
        //TODO: remove old repo that is not exist in githubRepositories
        public async Task ForceRescanUserRepositories(IwentysUser student)
        {
            List<GithubRepositoryInfoDto> githubRepositories = await _githubApiAccessor.GetUserRepositories(student.GithubUsername);
            IEnumerable<GithubProject> studentProjects = githubRepositories.Select(r => new GithubProject(student, r));
            foreach (GithubProject project in studentProjects)
                if (_studentProjectRepository.FindByIdAsync(project.Id) is null)
                    _studentProjectRepository.Update(project);
                else
                    await _studentProjectRepository.InsertAsync(project);

            await _unitOfWork.CommitAsync();
        }

        public async Task<GithubUser> CreateOrUpdate(int studentId)
        {
            IwentysUser student = await _userRepository.FindByIdAsync(studentId);
            GithubUser githubUserData = _githubUserRepository.Get().SingleOrDefault(gh => gh.StudentId == studentId);
            var exists = true;

            if (githubUserData is null)
            {
                exists = false;
                GithubUserInfoDto githubUser = await _githubApiAccessor.GetGithubUser(student.GithubUsername);
                ContributionFullInfo contributionFullInfo = await _githubApiAccessor.GetUserActivity(student.GithubUsername);

                githubUserData = new GithubUser
                {
                    StudentId = studentId,
                    Username = student.GithubUsername,
                    AvatarUrl = githubUser.AvatarUrl,
                    Bio = githubUser.Bio,
                    Company = githubUser.Bio,
                    ContributionFullInfo = contributionFullInfo
                };
            }

            IEnumerable<GithubProject> studentProjects = (await _githubApiAccessor.GetUserRepositories(student.GithubUsername))
                .Select(r => new GithubProject(student, r));

            if (exists)
            {
                foreach (GithubProject project in studentProjects)
                    if (_studentProjectRepository.FindByIdAsync(project.Id) is null)
                        _studentProjectRepository.Update(project);
                    else
                        await _studentProjectRepository.InsertAsync(project);

                githubUserData.ContributionFullInfo = await _githubApiAccessor.GetUserActivity(student.GithubUsername);
                _githubUserRepository.Update(githubUserData);
            }
            else
            {
                await _githubUserRepository.InsertAsync(githubUserData);
                foreach (GithubProject githubProjectEntity in studentProjects) await _studentProjectRepository.InsertAsync(githubProjectEntity);
            }

            await _unitOfWork.CommitAsync();

            return githubUserData;
        }

        public Task<List<GithubUser>> GetAllGithubUser()
        {
            return _githubUserRepository
                .Get()
                .ToListAsync();
        }

        public async Task<ContributionFullInfo> FindUserContributionOrEmpty(IwentysUser student, bool useCache = true)
        {
            if (student.GithubUsername is null) return ContributionFullInfo.Empty;

            GithubUser user = await FindGithubUser(student.Id, useCache);

            return user?.ContributionFullInfo ?? ContributionFullInfo.Empty;
        }


        public async Task<GithubUser> GetGithubUser(string username, bool useCache = true)
        {
            GithubUser result = await _githubUserRepository
                .Get()
                .SingleOrDefaultAsync(g => g.Username == username);

            if (!useCache)
            {
                IwentysUser student = await EnsureStudentWithGithub(username);
                result = await ForceRescanUser(student, result);
            }

            return result;
        }

        //TODO: why this is Find? Do we need this?
        public async Task<GithubUser> FindGithubUser(int studentId, bool useCache = true)
        {
            GithubUser result = await _githubUserRepository
                .Get()
                .SingleOrDefaultAsync(g => g.StudentId == studentId);

            //TODO: if null we... can try get from api?
            if (!useCache && result is not null)
            {
                IwentysUser student = await EnsureStudentWithGithub(studentId);
                result = await ForceRescanUser(student, result);
            }

            return result;
        }

        //TODO: wrap with domain entity?
        private async Task<IwentysUser> EnsureStudentWithGithub(int studentId)
        {
            IwentysUser student = await _userRepository.FindByIdAsync(studentId);
            if (student.GithubUsername is null)
                throw new InnerLogicException("Student do not link github");
            return student;
        }

        private async Task<IwentysUser> EnsureStudentWithGithub(string githubUsername)
        {
            IwentysUser iwentysUser = await _userRepository.Get().SingleAsync(s => s.GithubUsername == githubUsername);
            if (iwentysUser.GithubUsername is null)
                throw new InnerLogicException("Student do not link github");
            return iwentysUser;
        }
    }
}