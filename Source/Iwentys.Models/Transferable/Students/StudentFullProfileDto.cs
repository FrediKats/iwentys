using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;

namespace Iwentys.Models.Transferable.Students
{
    public class StudentFullProfileDto : StudentPartialProfileDto
    {
        public string Group { get; set; }

        public List<AchievementInfoDto> Achievements { get; set; }
        public string GuildName { get; set; }
        //public int StudyLeaderBoardPlace { get; set; }
        //public int CodingLeaderBoardPlace { get; set; }
        //public string SocialStatus { get; set; }
        //public string AdditionalLink { get; set; }

        public List<SubjectActivityInfoDto> SubjectActivityInfo { get; set; }
        public List<CodingActivityInfoDto> CodingActivityInfo { get; set; }

        public StudentFullProfileDto()
        {
        }

        public StudentFullProfileDto(Student student) : base(student)
        {
            Group = student.Group?.GroupName;
            //TODO:
            var random = new Random();

            Achievements = student.Achievements.SelectToList(AchievementInfoDto.Wrap);
            SubjectActivityInfo = student.SubjectActivities.SelectToList(sa => new SubjectActivityInfoDto(sa));
            GuildName = student.GuildMember?.Guild?.Title;

            CodingActivityInfo = Enumerable
                .Range(1, 12)
                .Select(i => new DateTime(2020, i, 1))
                .SelectToList(v => new CodingActivityInfoDto {Month = $"{v:M}", Activity = random.Next() % 100});
        }

        public string FormatFullInfo()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Format());
            if (!string.IsNullOrWhiteSpace(Group))
                builder.Append($" ({Group})");
            if (!string.IsNullOrWhiteSpace(GuildName))
                builder.Append($"\nGuild: {GuildName}");
            if (!string.IsNullOrWhiteSpace(GithubUsername))
                builder.Append($"\nGithub: {GithubUsername}");

            return builder.ToString();
        }
    }
}