using System.Collections.Generic;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Models.Tools;

namespace Iwentys.Database
{
    public static class AchievementList
    {
        public static readonly AchievementEntity AddGithubAchievement;
        public static readonly AchievementEntity BetaTester;
        public static readonly AchievementEntity QuestCreator;
        public static readonly AchievementEntity QuestComplete;
        public static readonly AchievementEntity TestTaskDone;

        private static readonly IdentifierGenerator IdentifierGenerator = new IdentifierGenerator();

        static AchievementList()
        {
            Achievements = new List<AchievementEntity>();
            AddGithubAchievement = Register(new AchievementEntity
            {
                Title = "Add github",
                Description = "Lorem",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            BetaTester = Register(new AchievementEntity
            {
                Title = "TP tester",
                Description = "Lorem",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            QuestCreator = Register(new AchievementEntity
            {
                Title = "Quest creator",
                Description = "For creating quest",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            QuestComplete = Register(new AchievementEntity
            {
                Title = "Quest done",
                Description = "Quest done",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            TestTaskDone = Register(new AchievementEntity
            {
                Title = "Test task done",
                Description = "Test task done",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });
        }

        public static List<AchievementEntity> Achievements { get; }

        private static AchievementEntity Register(AchievementEntity achievement)
        {
            achievement.Id = IdentifierGenerator.Next();
            Achievements.Add(achievement);
            return achievement;
        }
    }
}