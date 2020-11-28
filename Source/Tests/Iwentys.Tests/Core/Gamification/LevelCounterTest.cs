using Iwentys.Core.Gamification;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Gamification
{
    [TestFixture]
    public class LevelCounterTest
    {
        [Test]
        public void ZeroExperience_ShouldBeFirstLevel()
        {
            var levelCount = new LevelCounter(0);

            Assert.AreEqual(0, levelCount.Level);
        }

        [Test]
        public void HasSecondLevel_CorrectExperienceCountToNextLevel()
        {
            var levelCount = new LevelCounter(LevelCounter.ConvertLevelToExperienceBound(1) + 10);

            Assert.AreEqual(1, levelCount.Level);
            Assert.AreEqual(LevelCounter.ConvertLevelToExperienceBound(2), levelCount.ExperienceToNextLevel);
        }
    }
}