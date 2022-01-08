namespace Iwentys.AccountManagement;

public class MentorDto
{
    public int Id { get; set; }
    public bool IsLector { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string SecondName { get; set; }

    public MentorDto()
    {

    }

    public MentorDto(int id, bool isLector, string firstName, string middleName, string secondName)
    {
        IsLector = isLector;
        Id = id;
        FirstName = firstName;
        MiddleName = middleName;
        SecondName = secondName;
    }
};