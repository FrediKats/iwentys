using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models;

namespace Iwentys.Features.Guilds.Entities
{
    public class Guild
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Bio { get; private set; }
        public string ImageUrl { get; private set; }
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
                ImageUrl = arguments.LogoUrl,
                Title = arguments.Title,
                GuildType = GuildType.Pending
            };

            newGuild.Members.Add(new GuildMember(newGuild, creator, GuildMemberType.Creator));

            return newGuild;
        }

        public void Update(GuildMentor guildMentor, GuildUpdateRequestDto arguments)
        {
            Bio = arguments.Bio ?? Bio;
            ImageUrl = arguments.LogoUrl ?? ImageUrl;
            TestTaskLink = arguments.TestTaskLink ?? TestTaskLink;
            HiringPolicy = arguments.HiringPolicy ?? HiringPolicy;

            if (arguments.HiringPolicy == GuildHiringPolicy.Open)
                foreach (GuildMember guildMember in Members.Where(guildMember => guildMember.MemberType == GuildMemberType.Requested))
                    guildMember.Approve(guildMentor);
        }

        public void Approve(SystemAdminUser admin)
        {
            if (GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            GuildType = GuildType.Created;
        }

        public List<GuildMemberImpactDto> GetImpact()
        {
            return Members.Select(member => new GuildMemberImpactDto(member)).ToList();
        }
    }
}