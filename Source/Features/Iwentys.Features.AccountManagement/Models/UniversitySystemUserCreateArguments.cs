namespace Iwentys.Features.AccountManagement.Models
{
    public class UniversitySystemUserCreateArguments
    {
        private UniversitySystemUserCreateArguments()
        {
        }

        public UniversitySystemUserCreateArguments(int? id, string firstName, string middleName, string secondName) : this()
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            SecondName = secondName;
        }

        public int? Id { get; init; }

        public string FirstName { get; init; }
        public string MiddleName { get; init; }
        public string SecondName { get; init; }
    }
}