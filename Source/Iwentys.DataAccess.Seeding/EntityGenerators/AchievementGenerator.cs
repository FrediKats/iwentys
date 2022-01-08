using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess.Seeding
{
    public class AchievementGenerator : IEntityGenerator
    {
        public AchievementGenerator(List<Student> students, List<Guild> guilds)
        {
            StudentAchievementModels = students
                .Select(s => new StudentAchievement
                {
                    AchievementId = AchievementList.BetaTester.Id,
                    StudentId = s.Id,
                    GettingTimeUtc = DateTime.UtcNow
                }).ToList();

            GuildAchievementModels = guilds
                .Select(g => new GuildAchievement
                {
                    AchievementId = AchievementList.BetaTester.Id,
                    GuildId = g.Id,
                    GettingTimeUtc = DateTime.UtcNow
                }).ToList();
        }

        public List<GuildAchievement> GuildAchievementModels { get; set; }
        public List<StudentAchievement> StudentAchievementModels { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            //FYI: is this okay? We seed data not from this generator
            modelBuilder.Entity<Achievement>().HasData(AchievementList.Achievements);
            modelBuilder.Entity<StudentAchievement>().HasData(StudentAchievementModels);
            modelBuilder.Entity<GuildAchievement>().HasData(GuildAchievementModels);
        }
    }
}