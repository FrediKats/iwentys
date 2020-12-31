using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class AchievementGenerator : IEntityGenerator
    {
        public List<GuildAchievement> GuildAchievementModels { get; set; }
        public List<StudentAchievement> StudentAchievementModels { get; set; }

        public AchievementGenerator(List<Student> students, List<Guild> guilds)
        {
            StudentAchievementModels = students
                .Select(s => new StudentAchievement
                {
                    AchievementId = AchievementList.BetaTester.Id,
                    StudentId = s.Id,
                    CreationTimeUtc = DateTime.UtcNow
                }).ToList();

            GuildAchievementModels = guilds
                .Select(g => new GuildAchievement
                {
                    AchievementId = AchievementList.BetaTester.Id,
                    GuildId = g.Id,
                    CreationTimeUtc = DateTime.UtcNow
                }).ToList();
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            //FYI: is this okay? We seed data not from this generator
            modelBuilder.Entity<Achievement>().HasData(AchievementList.Achievements);
            modelBuilder.Entity<StudentAchievement>().HasData(StudentAchievementModels);
            modelBuilder.Entity<GuildAchievement>().HasData(GuildAchievementModels);
        }
    }
}