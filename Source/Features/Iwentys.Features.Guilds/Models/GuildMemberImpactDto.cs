using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models
{
    public record GuildMemberImpactDto
    {
        public GuildMemberImpactDto(
            IwentysUserInfoDto studentInfoDto,
            GuildMemberType memberType,
            ContributionFullInfo contribution) : this(studentInfoDto, memberType)
        {
            Contribution = contribution;
            TotalRate = contribution.Total;
        }

        public GuildMemberImpactDto(
            IwentysUserInfoDto studentInfoDto,
            GuildMemberType memberType) : this()
        {
            StudentInfoDto = studentInfoDto;
            MemberType = memberType;
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