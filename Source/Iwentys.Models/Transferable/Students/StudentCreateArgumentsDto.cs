using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable.Students
{
    public class StudentCreateArgumentsDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public UserType Role { get; set; }
        public string Group { get; set; }
        public string GithubUsername { get; set; }
        public int BarsPoints { get; set; }
    }
}