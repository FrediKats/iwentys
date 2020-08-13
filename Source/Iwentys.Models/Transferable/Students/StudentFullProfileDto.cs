using System;
using System.Collections.Generic;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;
using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable.Students
{
    public class StudentFullProfileDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public UserType Role { get; set; }
        public string Group { get; set; }
        public string GithubUsername { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastOnlineTime { get; set; }
        public int BarsPoints { get; set; }

        public List<AchievementInfoDto> Achievements { get; set; }
        //TODO: add some guild info?
        public string GuildName { get; set; }
        public int StudyLeaderBoardPlace { get; set; }
        public int CodingLeaderBoardPlace { get; set; }
        public string SocialStatus { get; set; }
        public string AdditionalLink { get; set; }
        
        //TODO: add Study diagrams
        //TODO: add GH coding stats for diagrams


        public StudentFullProfileDto()
        {
        }

        public StudentFullProfileDto(Student student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            MiddleName = student.MiddleName;
            SecondName = student.SecondName;
            Role = student.Role;
            Group = student.Group?.GroupName;
            GithubUsername = student.GithubUsername;
            CreationTime = student.CreationTime;
            LastOnlineTime = student.LastOnlineTime;
            BarsPoints = student.BarsPoints;
            Achievements = student.Achievements.SelectToList(AchievementInfoDto.Wrap);
        }
    }
}