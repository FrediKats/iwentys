using Iwentys.EntityManager.Domain.Accounts;
using Iwentys.EntityManager.PublicTypes;

namespace Iwentys.EntityManager.Domain;

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
    public virtual StudyGroup Group { get; set; }
}