using System.Collections.Generic;
using System.Text;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;

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
            Achievements = student.Achievements.SelectToList(AchievementInfoDto.Wrap);
            SubjectActivityInfo = student.SubjectActivities.SelectToList(sa => new SubjectActivityInfoDto(sa));
            GuildName = student.GuildMember?.Guild?.Title;

            if (student.GithubUserEntity is null || student.GithubUserEntity.ContributionFullInfo is null)
                CodingActivityInfo = new List<CodingActivityInfoDto>();
            else
                CodingActivityInfo = student.GithubUserEntity.ContributionFullInfo.PerMonthActivity().SelectToList(CodingActivityInfoDto.Wrap);
        }

        public string Group { get; set; }

        public List<AchievementInfoDto> Achievements { get; set; }

        public string GuildName { get; set; }
        //public int StudyLeaderBoardPlace { get; set; }
        //public int CodingLeaderBoardPlace { get; set; }
        //public string SocialStatus { get; set; }
        //public string AdditionalLink { get; set; }

        public List<SubjectActivityInfoDto> SubjectActivityInfo { get; set; }
        public List<CodingActivityInfoDto> CodingActivityInfo { get; set; }

        public string FormatFullInfo()
        {
            var builder = new StringBuilder();

            builder.Append(Format());
            if (!string.IsNullOrWhiteSpace(Group))
                builder.Append(" (").Append(Group).Append(')');
            if (!string.IsNullOrWhiteSpace(GuildName))
                builder.Append("\nGuild: ").Append(GuildName);
            if (!string.IsNullOrWhiteSpace(GithubUsername))
                builder.Append("\nGithub: ").Append(GithubUsername);

            return builder.ToString();
        }
    }
}