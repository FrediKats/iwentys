using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.GithubIntegration;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.WebService.Application;

public class GithubRepositoryApiAccessor
{
    private readonly IGithubApiAccessor _githubApiAccessor;
    private readonly IwentysDbContext _context;
    public readonly GithubUserApiAccessor UserApiApiAccessor;

    public GithubRepositoryApiAccessor(IGithubApiAccessor githubApiAccessor, GithubUserApiAccessor userApiApiAccessor, IwentysDbContext context)
    {
        _githubApiAccessor = githubApiAccessor;
        UserApiApiAccessor = userApiApiAccessor;
        _context = context;
    }

    public async Task<GithubRepositoryInfoDto> GetRepository(string username, string projectName, bool useCache = true)
    {
        if (!useCache)
        {
            GithubUser githubUser = await UserApiApiAccessor.GetGithubUser(username, useCache);
            await ForceRescanUserRepositories(githubUser);
        }

        GithubProject githubRepository = await _context
            .StudentProjects
            .SingleOrDefaultAsync(p => p.Owner == username && p.Name == projectName);

        if (githubRepository is null)
            return await _githubApiAccessor.GetRepository(username, projectName);

        return new GithubRepositoryInfoDto(githubRepository);
    }

    public async Task<GithubProject> GetRepositoryAsProject(string username, string projectName, bool useCache = true)
    {
        if (!useCache)
        {
            GithubUser githubUser = await UserApiApiAccessor.GetGithubUser(username, useCache);
            await ForceRescanUserRepositories(githubUser);
        }

        GithubProject githubRepository = await _context
            .StudentProjects
            .SingleOrDefaultAsync(p => p.Owner == username && p.Name == projectName);

        if (githubRepository is null)
        {
            var result = await _githubApiAccessor.GetRepository(username, projectName);
            GithubUser githubUser = await UserApiApiAccessor.GetGithubUser(username, useCache);
            return new GithubProject(githubUser, result);
        }

        return githubRepository;
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

        return await _context
            .StudentProjects
            .Where(p => p.OwnerUserId == githubUser.IwentysUserId)
            .Select(GithubRepositoryInfoDto.FromEntity)
            .ToListAsync();
    }

    public async Task ForceRescanUserRepositories(GithubUser githubUser)
    {
        List<GithubRepositoryInfoDto> githubRepositories = await _githubApiAccessor.GetUserRepositories(githubUser.Username);
        IEnumerable<GithubProject> studentProjects = githubRepositories.Select(r => new GithubProject(githubUser, r));
        foreach (GithubProject project in studentProjects)
            if (await _context.StudentProjects.FindAsync(project.Id) is null)
                _context.StudentProjects.Update(project);
            else
                _context.StudentProjects.Add(project);

        await _context.SaveChangesAsync();
    }
}