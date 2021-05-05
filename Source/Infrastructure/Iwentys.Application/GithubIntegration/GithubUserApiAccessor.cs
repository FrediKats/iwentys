using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.GithubIntegration.GithubIntegration
{
    public class GithubUserApiAccessor : IGithubUserApiAccessor
    {
        private readonly IGithubApiAccessor _githubApiAccessor;
        private readonly IGenericRepository<GithubUser> _githubUserRepository;
        private readonly IGenericRepository<GithubProject> _studentProjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<IwentysUser> _userRepository;

        public GithubUserApiAccessor(IGithubApiAccessor githubApiAccessor, IUnitOfWork unitOfWork)
        {
            _githubApiAccessor = githubApiAccessor;

            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
            _studentProjectRepository = _unitOfWork.GetRepository<GithubProject>();
            _githubUserRepository = _unitOfWork.GetRepository<GithubUser>();
        }

        public async Task<GithubUser> CreateOrUpdate(int studentId)
        {
            IwentysUser student = await _userRepository.GetById(studentId);
            GithubUser githubUserData = _githubUserRepository.Get().SingleOrDefault(gh => gh.IwentysUserId == studentId);
            var exists = true;

            if (githubUserData is null)
            {
                exists = false;
                GithubUserInfoDto githubUser = await _githubApiAccessor.GetGithubUser(student.GithubUsername);
                ContributionFullInfo contributionFullInfo = await _githubApiAccessor.GetUserActivity(student.GithubUsername);

                githubUserData = new GithubUser
                {
                    IwentysUserId = studentId,
                    Username = student.GithubUsername,
                    AvatarUrl = githubUser.AvatarUrl,
                    Bio = githubUser.Bio,
                    Company = githubUser.Bio,
                    ContributionFullInfo = contributionFullInfo
                };
            }

            IEnumerable<GithubProject> studentProjects = (await _githubApiAccessor.GetUserRepositories(student.GithubUsername))
                .Select(r => new GithubProject(githubUserData, r));

            if (exists)
            {
                foreach (GithubProject project in studentProjects)
                    if (_studentProjectRepository.FindByIdAsync(project.Id) is null)
                        _studentProjectRepository.Update(project);
                    else
                        _studentProjectRepository.Insert(project);

                githubUserData.ContributionFullInfo = await _githubApiAccessor.GetUserActivity(student.GithubUsername);
                _githubUserRepository.Update(githubUserData);
            }
            else
            {
                _githubUserRepository.Insert(githubUserData);
                foreach (GithubProject githubProjectEntity in studentProjects)
                    _studentProjectRepository.Insert(githubProjectEntity);
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

        public async Task<GithubUser> Get(int studentId, bool useCache = true)
        {
            return await FindGithubUser(studentId, useCache) ?? throw EntityNotFoundException.Create(typeof(GithubUser), studentId);
        }

        //FYI: why this is Find? Do we need this?
        public async Task<GithubUser> FindGithubUser(int studentId, bool useCache = true)
        {
            GithubUser result = await _githubUserRepository
                .Get()
                .SingleOrDefaultAsync(g => g.IwentysUserId == studentId);

            //FYI: if null we... can try get from api?
            if (!useCache && result is not null)
            {
                IwentysUser student = await EnsureStudentWithGithub(studentId);
                result = await ForceRescanUser(student, result);
            }

            return result;
        }

        public async Task<GithubUser> ForceRescanUser(IwentysUser student, GithubUser oldGithubUser)
        {
            GithubUserInfoDto githubUser = await _githubApiAccessor.GetGithubUser(student.GithubUsername);
            ContributionFullInfo contributionFullInfo = await _githubApiAccessor.GetUserActivity(student.GithubUsername);

            if (oldGithubUser is null)
            {
                var githubUserEntity = GithubUser.Create(student, githubUser, contributionFullInfo);
                _githubUserRepository.Insert(githubUserEntity);
                return githubUserEntity;
            }

            oldGithubUser.Update(githubUser, contributionFullInfo);
            _githubUserRepository.Update(oldGithubUser);
            await _unitOfWork.CommitAsync();
            return oldGithubUser;
        }

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