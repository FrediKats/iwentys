using System;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Study;

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
    public int? GroupId { get; set; }

    public static Student Create(StudentCreateArguments createArguments)
    {
        return new Student(id: createArguments.Id ?? 0, firstName: createArguments.FirstName, middleName: createArguments.MiddleName, secondName: createArguments.SecondName)
        {
            IsAdmin = createArguments.IsAdmin,
            GithubUsername = createArguments.GithubUsername,
            BarsPoints = createArguments.BarsPoints,
            AvatarUrl = createArguments.AvatarUrl,
            Type = createArguments.Type,
            GroupId = createArguments.GroupId
        };
    }
}