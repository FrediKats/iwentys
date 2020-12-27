using Iwentys.Features.Study.Domain;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Study
{
    [TestFixture]
    public class StudyGroupServiceTest
    {
        [Test]
        public void ParseGroupName_EnsureCorrectValue()
        {
            var groupAsString = "M3111";
            var groupName = new GroupName(groupAsString);

            Assert.AreEqual(1, groupName.Course);
            Assert.AreEqual(11, groupName.Number);
        }
    }
}