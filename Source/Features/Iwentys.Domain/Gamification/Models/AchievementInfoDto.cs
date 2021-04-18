using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Iwentys.Domain.Gamification.Models
{
    public class AchievementInfoDto
    {
        [JsonConstructor]
        public AchievementInfoDto(int id, string imageUrl, string title, string description, DateTime? gettingTimeUtc) : this()
        {
            Id = id;
            ImageUrl = imageUrl;
            Title = title;
            Description = description;
            GettingTimeUtc = gettingTimeUtc;
        }

        public AchievementInfoDto()
        {
        }

        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime? GettingTimeUtc { get; init; }
        public string ImageUrl { get; init; }

        public static Expression<Func<Achievement, AchievementInfoDto>> FromEntity =>
            achievement =>
                new AchievementInfoDto(
                    achievement.Id,
                    achievement.ImageUrl,
                    achievement.Title,
                    achievement.Description,
                    null);

        public static Expression<Func<StudentAchievement, AchievementInfoDto>> FromStudentsAchievement =>
            achievement =>
                new AchievementInfoDto(
                    achievement.AchievementId,
                    achievement.Achievement.ImageUrl,
                    achievement.Achievement.Title,
                    achievement.Achievement.Description,
                    achievement.GettingTimeUtc);

        public static Expression<Func<GuildAchievement, AchievementInfoDto>> FromGuildAchievement =>
            achievement =>
                new AchievementInfoDto(
                    achievement.AchievementId,
                    achievement.Achievement.ImageUrl,
                    achievement.Achievement.Title,
                    achievement.Achievement.Description,
                    achievement.GettingTimeUtc);
    }
}