using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Common.Tools;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.Guilds.Enums;

namespace Iwentys.Domain.Guilds.Models
{
    public class GuildProfileDto : GuildProfileShortInfoDto, IResultFormat
    {
        public GuildProfileDto()
        {
        }

        public GuildProfileDto(Guild guild) : base(guild)
        {
            Leader = guild.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member.To(s => new IwentysUserInfoDto(s));
            TestTasks = guild.TestTasks.SelectToList(GuildTestTaskInfoResponse.Wrap);
        }

        public static Expression<Func<Guild, GuildProfileDto>> FromEntity =>
            entity => new GuildProfileDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Bio = entity.Bio,
                ImmageUrl = entity.ImageUrl,
                TestTaskLink = entity.TestTaskLink,
                HiringPolicy = entity.HiringPolicy,
                GuildType = entity.GuildType,
                Leader = new IwentysUserInfoDto(entity.Members.Single(m => m.MemberType == GuildMemberType.Creator).Member),
                TestTasks = entity.TestTasks.Select(testTask => GuildTestTaskInfoResponse.Wrap(testTask)).ToList(),
                PinnedRepositories = entity.PinnedProjects.Select(p => new GithubRepositoryInfoDto(p.Project)).ToList(),
                GuildRatingList = entity.Members.Select(m => m.MemberImpact).ToList()
            };

        public IwentysUserInfoDto Leader { get; set; }
        public List<GuildTestTaskInfoResponse> TestTasks { get; set; }
        public List<GithubRepositoryInfoDto> PinnedRepositories { get; set; }
        public List<int> GuildRatingList { get; set; }

        public int GuildRating => GuildRatingList?.Sum() ?? 0;

        public string Format()
        {
            return $"{Title}";
        }
    }
}