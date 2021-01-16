using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models
{
    public record GuildMemberImpactDto
    {
        public GuildMemberImpactDto(
            IwentysUserInfoDto studentInfoDto,
            GuildMemberType memberType,
            ContributionFullInfo contribution) : this()
        {
            Contribution = contribution;
            TotalRate = contribution.Total;
            StudentInfoDto = studentInfoDto;
            MemberType = memberType;
        }

        public GuildMemberImpactDto(
            IwentysUserInfoDto studentInfoDto,
            GuildMemberType memberType,
            int totalRate) : this()
        {
            StudentInfoDto = studentInfoDto;
            MemberType = memberType;
            TotalRate = totalRate;
        }

        public GuildMemberImpactDto(GuildMember member)
        {
            StudentInfoDto = new IwentysUserInfoDto(member.Member);
            MemberType = member.MemberType;
            TotalRate = member.MemberImpact;
        }

        public GuildMemberImpactDto()
        {
        }

        public IwentysUserInfoDto StudentInfoDto { get; set; }
        public ContributionFullInfo Contribution { get; set; }
        public GuildMemberType MemberType { get; set; }
        public int TotalRate { get; init; }
    }
}