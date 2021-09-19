using Iwentys.Domain.AccountManagement;

namespace Iwentys.Modules.AccountManagement.Dtos
{
    public class UniversitySystemUserInfoDto
    {
        public UniversitySystemUserInfoDto(UniversitySystemUser user) : this()
        {
            Id = user.Id;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            SecondName = user.SecondName;
        }

        public UniversitySystemUserInfoDto()
        {
        }

        public int Id { get; init; }

        public string FirstName { get; init; }
        public string MiddleName { get; init; }
        public string SecondName { get; init; }

        public string GetFullName()
        {
            return $"{FirstName} {SecondName}";
        }
    }
}