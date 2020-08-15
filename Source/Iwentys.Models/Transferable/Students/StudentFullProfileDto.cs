using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;

namespace Iwentys.Models.Transferable.Students
{
    public class StudentFullProfileDto : StudentPartialProfileDto
    {
        public string Group { get; set; }

        public List<AchievementInfoDto> Achievements { get; set; }
        //TODO: add some guild info?
        //public string GuildName { get; set; }
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

            Achievements = student.Achievements.SelectToList(AchievementInfoDto.Wrap);
            SubjectActivityInfo = student.SubjectActivities.SelectToList(sa => new SubjectActivityInfoDto(sa));
            CodingActivityInfo = Enumerable
                .Range(1, 12)
                .Select(i => new DateTime(2020, i, 1))
                .SelectToList(v => new CodingActivityInfoDto {Month = $"{v:M}", Activity = 1});
        }
    }
}