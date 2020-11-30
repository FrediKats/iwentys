using System.Collections.Generic;
using System.Text;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Models.Transferable.Students
{
    public class StudentFullProfileDto : StudentPartialProfileDto
    {
        public StudentFullProfileDto()
        {
        }

        public StudentFullProfileDto(StudentEntity student) : base(student)
        {
            Group = student.Group?.GroupName;
            SubjectActivityInfo = student.SubjectActivities.SelectToList(sa => new SubjectActivityInfoResponse(sa));
            if (student.GuildMember?.Guild is not null)
                Guild = new GuildProfileShortInfoDto(student.GuildMember.Guild);

            if (student.GithubUserEntity is null || student.GithubUserEntity.ContributionFullInfo is null)
                CodingActivityInfo = new List<CodingActivityInfoResponse>();
            else
                CodingActivityInfo = student.GithubUserEntity.ContributionFullInfo.PerMonthActivity().SelectToList(CodingActivityInfoResponse.Wrap);
        }

        public string Group { get; set; }
        public GuildProfileShortInfoDto Guild { get; set; }

        //public int StudyLeaderBoardPlace { get; set; }
        //public int CodingLeaderBoardPlace { get; set; }
        //public string SocialStatus { get; set; }
        //public string AdditionalLink { get; set; }

        public List<SubjectActivityInfoResponse> SubjectActivityInfo { get; set; }
        public List<CodingActivityInfoResponse> CodingActivityInfo { get; set; }

        public string FormatFullInfo()
        {
            var builder = new StringBuilder();

            builder.Append(Format());
            if (!string.IsNullOrWhiteSpace(Group))
                builder.Append(" (").Append(Group).Append(')');
            if (!string.IsNullOrWhiteSpace(Guild?.Title))
                builder.Append("\nGuild: ").Append(Guild?.Title);
            if (!string.IsNullOrWhiteSpace(GithubUsername))
                builder.Append("\nGithub: ").Append(GithubUsername);

            return builder.ToString();
        }
    }
}