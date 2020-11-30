using System;
using Iwentys.Features.Achievements.Entities;
using Newtonsoft.Json;

namespace Iwentys.Features.Achievements.ViewModels
{
    public class AchievementInfoDto
    {
        [JsonConstructor]
        public AchievementInfoDto(string url, string name, string description, DateTime gettingTime)
        {
            Url = url;
            Name = name;
            Description = description;
            GettingTime = gettingTime;
        }

        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime GettingTime { get; set; }

        public static AchievementInfoDto Wrap(StudentAchievementEntity achievement)
        {
            return new AchievementInfoDto(
                achievement.Achievement.Url,
                achievement.Achievement.Title,
                achievement.Achievement.Description,
                achievement.GettingTime);
        }

        public static AchievementInfoDto Wrap(GuildAchievementEntity achievement)
        {
            return new AchievementInfoDto(
                achievement.Achievement.Url,
                achievement.Achievement.Title,
                achievement.Achievement.Description,
                achievement.GettingTime);
        }
    }
}