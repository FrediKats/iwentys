using Iwentys.Features.Study.Domain;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Study
{
    [TestFixture]
    public class GroupNameTest
    {
        [Test]
        public void ParseGroupName_EnsureCorrectValue()
        {
            var groupAsString = "M3111";
            var groupName = new GroupName(groupAsString);

            Assert.AreEqual(1, groupName.Course);
            Assert.AreEqual(11, groupName.Number);
        }

        [Test]
        public void ParseGroupNameWithWrongLetter_EnsureCorrectValue()
        {
            var groupWithRussian = "М3111";
            var groupWithLowerLetter = "m3111";

            Assert.AreEqual("M3111", new GroupName(groupWithRussian).Name);
            Assert.AreEqual("M3111", new GroupName(groupWithLowerLetter).Name);
        }
    }
}