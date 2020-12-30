using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class AchievementGenerator
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
                    GettingTime = DateTime.UtcNow
                }).ToList();

            GuildAchievementModels = guilds
                .Select(g => new GuildAchievement
                {
                    AchievementId = AchievementList.BetaTester.Id,
                    GuildId = g.Id,
                    GettingTime = DateTime.UtcNow
                }).ToList();
        }
    }
}