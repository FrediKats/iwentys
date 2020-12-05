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
        public List<GuildAchievementEntity> GuildAchievementModels { get; set; }
        public List<StudentAchievementEntity> StudentAchievementModels { get; set; }

        public AchievementGenerator(List<StudentEntity> students, List<GuildEntity> guilds)
        {
            StudentAchievementModels = students
                .Select(s => new StudentAchievementEntity
                {
                    AchievementId = AchievementList.BetaTester.Id,
                    StudentId = s.Id,
                    GettingTime = DateTime.UtcNow
                }).ToList();

            GuildAchievementModels = guilds
                .Select(g => new GuildAchievementEntity
                {
                    AchievementId = AchievementList.BetaTester.Id,
                    GuildId = g.Id,
                    GettingTime = DateTime.UtcNow
                }).ToList();
        }
    }
}