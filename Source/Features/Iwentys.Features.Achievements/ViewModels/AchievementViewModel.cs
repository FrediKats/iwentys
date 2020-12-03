using System;
using Iwentys.Features.Achievements.Entities;
using Newtonsoft.Json;

namespace Iwentys.Features.Achievements.ViewModels
{
    public class AchievementViewModel
    {
        [JsonConstructor]
        public AchievementViewModel(string imageUrl, string name, string description, DateTime gettingTime)
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

        public static AchievementViewModel Wrap(StudentAchievementEntity achievement)
        {
            return new AchievementViewModel(
                achievement.Achievement.Url,
                achievement.Achievement.Title,
                achievement.Achievement.Description,
                achievement.GettingTime);
        }

        public static AchievementViewModel Wrap(GuildAchievementEntity achievement)
        {
            return new AchievementViewModel(
                achievement.Achievement.Url,
                achievement.Achievement.Title,
                achievement.Achievement.Description,
                achievement.GettingTime);
        }
    }
}