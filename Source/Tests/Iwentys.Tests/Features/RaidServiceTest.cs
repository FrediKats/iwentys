using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    [TestFixture]
    public class RaidServiceTest
    {
        [Test]
        public async Task CreateRaid_ShouldExists()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser(true);
            RaidProfileDto raid = await testCase.RaidService.Create(user, new RaidCreateArguments());

            RaidProfileDto createdRaid = await testCase.RaidService.Get(raid.Id);
            Assert.IsNotNull(createdRaid);
        }
    }
}