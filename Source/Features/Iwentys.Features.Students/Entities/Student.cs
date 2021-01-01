using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Students.Entities
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; init; }

        public string FirstName { get; init; }
        public string MiddleName { get; init; }
        public string SecondName { get; init; }
        public StudentRole Role { get; set; }
        public StudentType Type { get; init; }
        public int? GroupId { get; set; }
        public string GithubUsername { get; set; }
        public DateTime CreationTime { get; init; }
        public DateTime LastOnlineTime { get; set; }
        public int BarsPoints { get; set; }
        public string AvatarUrl { get; set; }

        public DateTime GuildLeftTime { get; set; }

        public Student()
        {
        }

        public Student(int id, string firstName, string middleName, string secondName) : this()
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            SecondName = secondName;
            Role = StudentRole.Common;
            CreationTime = DateTime.UtcNow;
            LastOnlineTime = DateTime.UtcNow;
            GuildLeftTime = DateTime.MinValue;
        }

        public static Student CreateFromIsu(int id, string firstName, string secondName)
        {
            return CreateFromIsu(id, firstName, null, secondName);
        }

        public static Student CreateFromIsu(int id, string firstName, string middleName, string secondName, int? groupId = null)
        {
            return new Student(id, firstName, middleName, secondName)
            {
                GroupId = groupId
            };
        }

        public void MakeGroupAdmin(SystemAdminUser initiator) => Role = StudentRole.GroupAdmin;
        //TODO: do not allow admin to kick other admin
        public void MakeCommonMember(SystemAdminUser initiator) => Role = StudentRole.Common;
    }
}