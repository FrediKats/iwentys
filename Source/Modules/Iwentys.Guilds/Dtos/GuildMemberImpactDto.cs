using Iwentys.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManager.ApiClient;
using Iwentys.WebService.Application;

namespace Iwentys.Guilds;

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
        StudentInfoDto = EntityManagerApiDtoMapper.Map(member.Member);
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