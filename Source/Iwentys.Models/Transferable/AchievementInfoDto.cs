using System;
using Iwentys.Models.Entities.Gamification;

namespace Iwentys.Models.Transferable
{
    public class AchievementInfoDto
    {
        //TODO: replace with URL or badge type
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime GettingTime { get; set; }

        public static AchievementInfoDto Wrap(StudentAchievementEntity achievement)
        {
            return new AchievementInfoDto
            {
                Name = achievement.Achievement.Title,
                Description = achievement.Achievement.Description,
                GettingTime = achievement.GettingTime
            };
        }

        public static AchievementInfoDto Wrap(GuildAchievementModel achievement)
        {
            return new AchievementInfoDto
            {
                Name = achievement.Achievement.Title,
                Description = achievement.Achievement.Description,
                GettingTime = achievement.GettingTime
            };
        }
    }
}