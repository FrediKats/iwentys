using System;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Study.Enums;

namespace Iwentys.Features.Study.Entities
{
    public class Student : IwentysUser
    {
        public StudentType Type { get; init; }
        public virtual StudyGroupMember GroupMember { get; set; }


        public Student()
        {
        }

        public Student(int id, string firstName, string middleName, string secondName) : this()
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            SecondName = secondName;
            CreationTime = DateTime.UtcNow;
            LastOnlineTime = DateTime.UtcNow;
            GuildLeftTime = DateTime.MinValue;
        }

        public static Student CreateFromIsu(int id, string firstName, string secondName)
        {
            return CreateFromIsu(id, firstName, null, secondName);
        }

        public static Student CreateFromIsu(int id, string firstName, string middleName, string secondName)
        {
            return new Student(id, firstName, middleName, secondName)
            {
                //TODO: it's not work any more, meh
                //GroupId = -1
            };
        }
    }
}