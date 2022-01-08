using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Raids;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features;

[TestFixture]
public class RaidServiceTest
{
    [Test]
    public void CreateRaid_ShouldExists()
    {
        TestCaseContext testCase = TestCaseContext.Case();
        IwentysUser user = testCase.AccountManagementTestCaseContext.WithIwentysUser(true);

        var raid = Raid.CreateCommon(user, new RaidCreateArguments());

        Assert.IsNotNull(raid);
    }
}