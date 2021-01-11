using System;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Study.Enums;
using Iwentys.Features.Study.Models.Students;

namespace Iwentys.Features.Study.Entities
{
    public class Student : IwentysUser
    {
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
        }

        public StudentType Type { get; init; }
        public virtual StudyGroupMember GroupMember { get; set; }

        public static Student CreateFromIsu(int id, string firstName, string secondName)
        {
            return CreateFromIsu(id, firstName, null, secondName);
        }

        public static Student CreateFromIsu(int id, string firstName, string middleName, string secondName)
        {
            return new Student(id, firstName, middleName, secondName);
        }

        public static Student CreateFromIsu(StudentCreateArguments createArguments)
        {
            return new Student(id: createArguments.Id ?? 0, firstName: createArguments.FirstName, middleName: createArguments.MiddleName, secondName: createArguments.SecondName)
            {
                IsAdmin = createArguments.IsAdmin,
                GithubUsername = createArguments.GithubUsername,
                BarsPoints = createArguments.BarsPoints,
                AvatarUrl = createArguments.AvatarUrl
            };
        }
    }
}