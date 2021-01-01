using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Guilds.Models
{
    public record GuildMemberImpactDto
    {
        public GuildMemberImpactDto(
            StudentInfoDto studentInfoDto,
            GuildMemberType memberType,
            ContributionFullInfo contribution) : this(studentInfoDto, memberType)
        {
            Contribution = contribution;
            TotalRate = contribution.Total;
        }

        public GuildMemberImpactDto(
            StudentInfoDto studentInfoDto,
            GuildMemberType memberType) : this()
        {
            StudentInfoDto = studentInfoDto;
            MemberType = memberType;
        }

        public GuildMemberImpactDto()
        {
        }
        
        public StudentInfoDto StudentInfoDto { get; set; }
        public ContributionFullInfo Contribution { get; set; }
        public GuildMemberType MemberType { get; set; }
        public int TotalRate { get; init; }
    }
}