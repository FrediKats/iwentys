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

        public int Id { get; init; }
        public string ImageUrl { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public DateTime GettingTime { get; init; }

        public static Expression<Func<StudentAchievement, AchievementDto>> FromStudentsAchievement =>
            achievement =>
                new AchievementDto(
                    achievement.AchievementId,
                    achievement.Achievement.Url,
                    achievement.Achievement.Title,
                    achievement.Achievement.Description,
                    achievement.CreationTimeUtc);

        public static Expression<Func<GuildAchievement, AchievementDto>> FromGuildAchievement =>
            achievement =>
                new AchievementDto(
                    achievement.AchievementId,
                    achievement.Achievement.Url,
                    achievement.Achievement.Title,
                    achievement.Achievement.Description,
                    achievement.CreationTimeUtc);
    }
}