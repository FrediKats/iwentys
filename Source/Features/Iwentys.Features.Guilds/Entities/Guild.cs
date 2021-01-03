using System.Collections.Generic;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models;

namespace Iwentys.Features.Guilds.Entities
{
    public class Guild
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Bio { get; private set; }
        public string LogoUrl { get; private set; }
        public string TestTaskLink { get; private set; }

        public GuildHiringPolicy HiringPolicy { get; set; }
        public GuildType GuildType { get; private set; }

        public virtual List<GuildMember> Members { get; init; } = new List<GuildMember>();
        public virtual List<GuildPinnedProject> PinnedProjects { get; init; } = new List<GuildPinnedProject>();
        public virtual List<GuildTestTaskSolution> TestTasks { get; init; } = new List<GuildTestTaskSolution>();
        
        public static Guild Create(IwentysUser creator, GuildCreateRequestDto arguments)
        {
            var newGuild = new Guild
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                LogoUrl = arguments.LogoUrl,
                Title = arguments.Title,
                GuildType = GuildType.Pending
            };

            newGuild.Members.Add(new GuildMember(newGuild, creator, GuildMemberType.Creator));

            return newGuild;
        }

        public void Update(GuildUpdateRequestDto arguments)
        {
            Bio = arguments.Bio ?? Bio;
            LogoUrl = arguments.LogoUrl ?? LogoUrl;
            TestTaskLink = arguments.TestTaskLink ?? TestTaskLink;
            HiringPolicy = arguments.HiringPolicy ?? HiringPolicy;
        }

        public void Approve(SystemAdminUser admin)
        {
            if (GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            GuildType = GuildType.Created;
        }
    }
}