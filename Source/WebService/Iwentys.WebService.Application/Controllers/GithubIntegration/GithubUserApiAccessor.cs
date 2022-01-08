using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.WebService.Application;

public class GithubUserApiAccessor : IGithubUserApiAccessor
{
    private readonly IGithubApiAccessor _githubApiAccessor;
    private readonly IwentysDbContext _context;

    public GithubUserApiAccessor(IGithubApiAccessor githubApiAccessor, IwentysDbContext context)
    {
        _githubApiAccessor = githubApiAccessor;
        _context = context;
    }

    public async Task<GithubUser> CreateOrUpdate(int studentId)
    {
        IwentysUser student = await _context.IwentysUsers.GetById(studentId);
        GithubUser githubUserData = _context.GithubUsersData.SingleOrDefault(gh => gh.IwentysUserId == studentId);
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
                if (_context.StudentProjects.Find(project.Id) is null)
                    _context.StudentProjects.Update(project);
                else
                    _context.StudentProjects.Add(project);

            githubUserData.ContributionFullInfo = await _githubApiAccessor.GetUserActivity(student.GithubUsername);
            _context.GithubUsersData.Update(githubUserData);
        }
        else
        {
            _context.GithubUsersData.Add(githubUserData);
            foreach (GithubProject githubProjectEntity in studentProjects)
                _context.StudentProjects.Add(githubProjectEntity);
        }

        await _context.SaveChangesAsync();

        return githubUserData;
    }

    public Task<List<GithubUser>> GetAllGithubUser()
    {
        return _context.GithubUsersData
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
        GithubUser result = await _context.GithubUsersData
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
        GithubUser result = await _context.GithubUsersData
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
            _context.GithubUsersData.Add(githubUserEntity);
            return githubUserEntity;
        }

        oldGithubUser.Update(githubUser, contributionFullInfo);
        _context.GithubUsersData.Update(oldGithubUser);
        await _context.SaveChangesAsync();
        return oldGithubUser;
    }

    private async Task<IwentysUser> EnsureStudentWithGithub(int studentId)
    {
        IwentysUser student = await _context.IwentysUsers.FindAsync(studentId);
        if (student.GithubUsername is null)
            throw new InnerLogicException("Student do not link github");
        return student;
    }

    private async Task<IwentysUser> EnsureStudentWithGithub(string githubUsername)
    {
        IwentysUser iwentysUser = await _context.IwentysUsers.SingleAsync(s => s.GithubUsername == githubUsername);
        if (iwentysUser.GithubUsername is null)
            throw new InnerLogicException("Student do not link github");
        return iwentysUser;
    }
}