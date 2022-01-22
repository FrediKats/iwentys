using Iwentys.EntityManager.PublicTypes;

namespace Iwentys.EntityManager.WebApiDtos;

public class TeacherDto
{
    public int Id { get; set; }
    public TeacherType TeacherType { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string SecondName { get; set; }

    public TeacherDto()
    {
    }

    public TeacherDto(int id, TeacherType teacherType, string firstName, string middleName, string secondName)
    {
        TeacherType = teacherType;
        Id = id;
        FirstName = firstName;
        MiddleName = middleName;
        SecondName = secondName;
    }
};