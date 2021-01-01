using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Iwentys.Features.Achievements.Entities;

namespace Iwentys.Features.Achievements.Models
{
    public class AchievementDto
    {
        [JsonConstructor]
        public AchievementDto(int id, string imageUrl, string name, string description, DateTime gettingTime)
        {
            Id = id;
            ImageUrl = imageUrl;
            Name = name;
            Description = description;
            GettingTime = gettingTime;
        }

        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime GettingTime { get; set; }

        public static Expression<Func<StudentAchievement, AchievementDto>> FromEntity =>
            achievement =>
                new AchievementDto(
                    achievement.AchievementId,
                    achievement.Achievement.Url,
                    achievement.Achievement.Title,
                    achievement.Achievement.Description,
                    achievement.CreationTimeUtc);


        public static AchievementDto Wrap(StudentAchievement achievement)
        {
            return new AchievementDto(
                achievement.AchievementId,
                achievement.Achievement.Url,
                achievement.Achievement.Title,
                achievement.Achievement.Description,
                achievement.CreationTimeUtc);
        }

        public static AchievementDto Wrap(GuildAchievement achievement)
        {
            return new AchievementDto(
                achievement.AchievementId,
                achievement.Achievement.Url,
                achievement.Achievement.Title,
                achievement.Achievement.Description,
                achievement.CreationTimeUtc);
        }
    }
}