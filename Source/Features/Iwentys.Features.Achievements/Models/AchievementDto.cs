using System;
using Iwentys.Features.Achievements.Entities;
using Newtonsoft.Json;

namespace Iwentys.Features.Achievements.Models
{
    public class AchievementDto
    {
        [JsonConstructor]
        public AchievementDto(string imageUrl, string name, string description, DateTime gettingTime)
        {
            ImageUrl = imageUrl;
            Name = name;
            Description = description;
            GettingTime = gettingTime;
        }

        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime GettingTime { get; set; }

        public static AchievementDto Wrap(StudentAchievementEntity achievement)
        {
            return new AchievementDto(
                achievement.Achievement.Url,
                achievement.Achievement.Title,
                achievement.Achievement.Description,
                achievement.GettingTime);
        }

        public static AchievementDto Wrap(GuildAchievementEntity achievement)
        {
            return new AchievementDto(
                achievement.Achievement.Url,
                achievement.Achievement.Title,
                achievement.Achievement.Description,
                achievement.GettingTime);
        }
    }
}