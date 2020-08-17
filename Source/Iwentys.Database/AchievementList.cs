using System.Collections.Generic;
using Iwentys.Models.Entities.Gamification;

namespace Iwentys.Database
{
    public static class AchievementList
    {
        public static List<AchievementModel> Achievements { get; }
        public static readonly AchievementModel AddGithubAchievement;

        static AchievementList()
        {
            Achievements = new List<AchievementModel>();
            AddGithubAchievement = Register(new AchievementModel
            {
                Id = 1,
                Title = "Add github",
                Description = "Lorem"
            });

            AddGithubAchievement = Register(new AchievementModel
            {
                Id = 2,
                Title = "TP tester",
                Description = "Lorem"
            });
        }

        private static AchievementModel Register(AchievementModel achievement)
        {
            Achievements.Add(achievement);
            return achievement;
        }
    }
}