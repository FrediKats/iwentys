using System.Collections.Generic;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string LogoUrl { get; set; }
        public string TestTaskLink { get; set; }

        public GuildHiringPolicy HiringPolicy { get; set; }
        public GuildType GuildType { get; set; }

        public virtual List<GuildMemberEntity> Members { get; set; } = new List<GuildMemberEntity>();
        public virtual List<GuildPinnedProjectEntity> PinnedProjects { get; set; } = new List<GuildPinnedProjectEntity>();
        public virtual List<GuildTestTaskSolutionEntity> TestTasks { get; set; } = new List<GuildTestTaskSolutionEntity>();
        
        public virtual List<GuildAchievementEntity> Achievements { get; set; } = new List<GuildAchievementEntity>();

        public static GuildEntity Create(StudentEntity creator, GuildCreateRequestDto arguments)
        {
            var newGuild = new GuildEntity
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                LogoUrl = arguments.LogoUrl,
                Title = arguments.Title,
                GuildType = GuildType.Pending
            };

            newGuild.Members = new List<GuildMemberEntity>
            {
                new GuildMemberEntity(newGuild, creator, GuildMemberType.Creator)
            };

            return newGuild;
        }
    }
}