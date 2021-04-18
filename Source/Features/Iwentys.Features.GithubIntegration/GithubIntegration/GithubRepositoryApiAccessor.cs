using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.GithubIntegration.GithubIntegration
{
    public class GithubRepositoryApiAccessor
    {
        private readonly IGithubApiAccessor _githubApiAccessor;

        private readonly IGenericRepository<GithubProject> _studentProjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public readonly GithubUserApiAccessor UserApiApiAccessor;

        public GithubRepositoryApiAccessor(IGithubApiAccessor githubApiAccessor, IUnitOfWork unitOfWork, GithubUserApiAccessor userApiApiAccessor)
        {
            _githubApiAccessor = githubApiAccessor;
            UserApiApiAccessor = userApiApiAccessor;

            _unitOfWork = unitOfWork;
            _studentProjectRepository = _unitOfWork.GetRepository<GithubProject>();
        }

        public async Task<GithubRepositoryInfoDto> GetRepository(string username, string projectName, bool useCache = true)
        {
            if (!useCache)
            {
                GithubUser githubUser = await UserApiApiAccessor.GetGithubUser(username, useCache);
                await ForceRescanUserRepositories(githubUser);
            }

            GithubProject githubRepository = await _studentProjectRepository
                .Get()
                .SingleOrDefaultAsync(p => p.Owner == username && p.Name == projectName);

            if (githubRepository is null)
                return await _githubApiAccessor.GetRepository(username, projectName);

            return new GithubRepositoryInfoDto(githubRepository);
        }

        public async Task<List<GithubRepositoryInfoDto>> GetStudentRepositories(int studentId, bool useCache = true)
        {
            GithubUser githubUser = await UserApiApiAccessor.Get(studentId, useCache);

            return await GetUserRepositories(githubUser.Username, useCache);
        }

        public async Task<List<GithubRepositoryInfoDto>> GetUserRepositories(string username, bool useCache = true)
        {
            GithubUser githubUser = await UserApiApiAccessor.GetGithubUser(username, useCache);

            if (!useCache) await ForceRescanUserRepositories(githubUser);

            return await _studentProjectRepository
                .Get()
                .Where(p => p.OwnerUserId == githubUser.IwentysUserId)
                .Select(GithubRepositoryInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task ForceRescanUserRepositories(GithubUser githubUser)
        {
            List<GithubRepositoryInfoDto> githubRepositories = await _githubApiAccessor.GetUserRepositories(githubUser.Username);
            IEnumerable<GithubProject> studentProjects = githubRepositories.Select(r => new GithubProject(githubUser, r));
            foreach (GithubProject project in studentProjects)
                if (_studentProjectRepository.FindByIdAsync(project.Id) is null)
                    _studentProjectRepository.Update(project);
                else
                    _studentProjectRepository.Insert(project);

            await _unitOfWork.CommitAsync();
        }
    }
}