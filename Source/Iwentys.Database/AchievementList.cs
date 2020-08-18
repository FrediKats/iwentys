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
                Description = "Lorem"
            });

            BetaTester = Register(new AchievementModel
            {
                Title = "TP tester",
                Description = "Lorem"
            });

            QuestCreator = Register(new AchievementModel
            {
                Title = "Quest creator",
                Description = "For creating quest"
            });

            QuestComplete = Register(new AchievementModel
            {
                Title = "Quest done",
                Description = "Quest done"
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