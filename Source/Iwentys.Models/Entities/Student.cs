using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public UserType Role { get; set; }
        public int? GroupId { get; set; }
        public StudyGroup Group { get; set; }
        public string GithubUsername { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastOnlineTime { get; set; }
        public int BarsPoints { get; set; }

        public DateTime GuildLeftTime { get; set; }
        public GuildMember GuildMember { get; set; }

        public List<StudentAchievementEntity> Achievements { get; set; }
        public List<SubjectActivity> SubjectActivities { get; set; }

        public static Student CreateFromIsu(int id, string firstName, string secondName, StudyGroup group = null)
        {
            return CreateFromIsu(id, firstName, null, secondName, group?.Id);
        }

        public static Student CreateFromIsu(int id, string firstName, string middleName, string secondName, int? groupId = null)
        {
            return new Student
            {
                Id = id,
                FirstName = firstName,
                MiddleName = middleName,
                SecondName = secondName,
                Role = UserType.Common,
                GroupId = groupId,
                CreationTime = DateTime.UtcNow,
                LastOnlineTime = DateTime.UtcNow,
                GuildLeftTime = DateTime.MinValue
            };
        }
    }
}