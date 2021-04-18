using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Guilds.Models;

namespace Iwentys.Domain.Guilds
{
    public class Guild
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }
        public string TestTaskLink { get; private set; }

        public GuildHiringPolicy HiringPolicy { get; set; }
        public GuildType GuildType { get; private set; }

        public virtual List<GuildMember> Members { get; init; } = new List<GuildMember>();
        public virtual List<GuildPinnedProject> PinnedProjects { get; init; } = new List<GuildPinnedProject>();
        public virtual List<GuildTestTaskSolution> TestTasks { get; init; } = new List<GuildTestTaskSolution>();

        public static Guild Create(IwentysUser creator, Guild userCurrentGuild, GuildCreateRequestDto arguments)
        {
            if (userCurrentGuild is not null)
                throw new InnerLogicException("Student already in guild");

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

        public void Update(IwentysUser user, GuildUpdateRequestDto arguments)
        {
            GuildMentor mentor = EnsureIsGuildMentor(user);

            Bio = arguments.Bio ?? Bio;
            ImageUrl = arguments.LogoUrl ?? ImageUrl;
            TestTaskLink = arguments.TestTaskLink ?? TestTaskLink;
            HiringPolicy = arguments.HiringPolicy ?? HiringPolicy;

            if (arguments.HiringPolicy == GuildHiringPolicy.Open)
                foreach (GuildMember guildMember in Members.Where(guildMember => guildMember.MemberType == GuildMemberType.Requested))
                    guildMember.Approve(mentor);
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

        public GuildMentor EnsureIsGuildMentor(IwentysUser user)
        {
            GuildMember membership = Members.FirstOrDefault(m => m.MemberId == user.Id);

            if (membership is null)
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, Id);

            return new GuildMentor(user, this, membership.MemberType);
        }

        //TODO: rework
        public async Task<List<GuildMemberImpactDto>> GetMemberImpacts(IGithubUserApiAccessor _githubUserApiAccessor)
        {
            //FYI: optimization is need
            var result = new List<GuildMemberImpactDto>();
            foreach (GuildMember member in Members)
            {
                ContributionFullInfo contributionFullInfo = await _githubUserApiAccessor.FindUserContributionOrEmpty(member.Member);
                result.Add(new GuildMemberImpactDto(new IwentysUserInfoDto(member.Member), member.MemberType, contributionFullInfo));
            }

            return result;
        }
    }
}