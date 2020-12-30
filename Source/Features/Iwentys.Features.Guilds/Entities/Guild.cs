using System.Collections.Generic;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class Guild
    {
        public int Id { get; set; }
        public string Title { get;  set; }
        public string Bio { get; set; }
        public string LogoUrl { get; set; }
        public string TestTaskLink { get; set; }

        public GuildHiringPolicy HiringPolicy { get; set; }
        public GuildType GuildType { get; set; }

        public virtual List<GuildMember> Members { get; set; } = new List<GuildMember>();
        public virtual List<GuildPinnedProject> PinnedProjects { get; set; } = new List<GuildPinnedProject>();
        public virtual List<GuildTestTaskSolution> TestTasks { get; set; } = new List<GuildTestTaskSolution>();
        
        public virtual List<GuildAchievement> Achievements { get; set; } = new List<GuildAchievement>();

        public static Guild Create(Student creator, GuildCreateRequestDto arguments)
        {
            var newGuild = new Guild
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                LogoUrl = arguments.LogoUrl,
                Title = arguments.Title,
                GuildType = GuildType.Pending
            };

            newGuild.Members = new List<GuildMember>
            {
                new GuildMember(newGuild, creator, GuildMemberType.Creator)
            };

            return newGuild;
        }
    }
}