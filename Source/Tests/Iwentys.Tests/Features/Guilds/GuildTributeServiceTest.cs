using System.Linq;
using Iwentys.DataAccess.Seeding;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Guilds;

[TestFixture]
public class GuildTributeServiceTest
{
    [Test]
    public void CreateTribute_TributeExists()
    {
        TestCaseContext context = TestCaseContext.Case();
        IwentysUser student = context.AccountManagementTestCaseContext.WithIwentysUser();
        IwentysUser admin = context.AccountManagementTestCaseContext.WithIwentysUser(true);
        var guild = Guild.Create(admin, null, GuildFaker.Instance.GetGuildCreateArguments());
        guild.Members.Add(new GuildMember(guild, student, GuildMemberType.Member));

        var newGithubUser = new GithubUser
        {
            IwentysUserId = student.Id,
            Username = student.GithubUsername
        };

        GithubUser githubUser = newGithubUser;
        GithubProject project = context.GithubTestCaseContext.WithStudentProject(student, githubUser);

        var tribute = Tribute.Create(guild, student, project);

        Assert.True(guild.Tributes.Any(t => t.Project.Id == project.Id));
    }

    [Test]
    public void CancelTribute_DoNotReturnForMentorAndReturnForStudent()
    {
        TestCaseContext context = TestCaseContext.Case();
        IwentysUser student = context.AccountManagementTestCaseContext.WithIwentysUser();
        IwentysUser admin = context.AccountManagementTestCaseContext.WithIwentysUser(true);
        var guild = Guild.Create(admin, null, GuildFaker.Instance.GetGuildCreateArguments());

        var newGithubUser = new GithubUser
        {
            IwentysUserId = student.Id,
            Username = student.GithubUsername
        };

        GithubUser githubUser = newGithubUser;
        GithubProject project = context.GithubTestCaseContext.WithStudentProject(student, githubUser);

        var tribute = Tribute.Create(guild, student, project);
        tribute.SetCanceled();

        Assert.IsEmpty(guild.Tributes.Where(t => t.State == TributeState.Active));
        Assert.True(guild.Tributes.Any(t => t.Project.Id == project.Id));
    }

    [Test]
    public void CompleteTribute_DoNotReturnForMentorAndChangeState()
    {
        TestCaseContext context = TestCaseContext.Case();
        IwentysUser student = context.AccountManagementTestCaseContext.WithIwentysUser();
        IwentysUser admin = context.AccountManagementTestCaseContext.WithIwentysUser(true);

        var guild = Guild.Create(admin, null, GuildFaker.Instance.GetGuildCreateArguments());
        guild.Members.Add(new GuildMember(guild, student, GuildMemberType.Mentor));

        var newGithubUser = new GithubUser
        {
            IwentysUserId = student.Id,
            Username = student.GithubUsername
        };

        GithubUser githubUser = newGithubUser;
        GithubProject project = context.GithubTestCaseContext.WithStudentProject(student, githubUser);

        var tribute = Tribute.Create(guild, student, project);

        tribute.SetCompleted(student.Id, GuildFaker.Instance.NewTributeCompleteRequest(tribute.Project.Id));

        Assert.IsEmpty(guild.Tributes.Where(t => t.State == TributeState.Active));
        Assert.AreEqual(TributeState.Completed, tribute.State);
    }
}