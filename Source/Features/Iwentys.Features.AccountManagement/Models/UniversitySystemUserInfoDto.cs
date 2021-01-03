using Iwentys.Features.AccountManagement.Entities;

namespace Iwentys.Features.AccountManagement.Models
{
    public class UniversitySystemUserInfoDto
    {
        public int Id { get; init; }

        public string FirstName { get; init; }
        public string MiddleName { get; init; }
        public string SecondName { get; init; }

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

        public string GetFullName()
        {
            return $"{FirstName} {SecondName}";
        }
    }
}