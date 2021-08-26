namespace Iwentys.Modules.AccountManagement.Mentors.Dtos
{
    public class MentorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        
        public MentorDto()
        {
            
        }

        public MentorDto(int id, string firstName, string middleName, string secondName)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            SecondName = secondName;
        }
    };
}