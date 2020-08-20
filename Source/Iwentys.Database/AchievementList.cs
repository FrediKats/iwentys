using System.Collections.Generic;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Models.Tools;

namespace Iwentys.Database
{
    public static class AchievementList
    {
        public static List<AchievementModel> Achievements { get; }
        public static readonly AchievementModel AddGithubAchievement;
        public static readonly AchievementModel BetaTester;
        public static readonly AchievementModel QuestCreator;
        public static readonly AchievementModel QuestComplete;

        private static readonly IdentifierGenerator IdentifierGenerator = new IdentifierGenerator();

        static AchievementList()
        {
            Achievements = new List<AchievementModel>();
            AddGithubAchievement = Register(new AchievementModel
            {
                Title = "Add github",
                Description = "Lorem",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            BetaTester = Register(new AchievementModel
            {
                Title = "TP tester",
                Description = "Lorem",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            QuestCreator = Register(new AchievementModel
            {
                Title = "Quest creator",
                Description = "For creating quest",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            QuestComplete = Register(new AchievementModel
            {
                Title = "Quest done",
                Description = "Quest done",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });
        }

        private static AchievementModel Register(AchievementModel achievement)
        {
            achievement.Id = IdentifierGenerator.Next();
            Achievements.Add(achievement);
            return achievement;
        }
    }
}