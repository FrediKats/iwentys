using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Features.AccountManagement.Models;

namespace Iwentys.Features.AccountManagement.Entities
{
    public class UniversitySystemUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; init; }

        public string FirstName { get; init; }
        public string MiddleName { get; init; }
        public string SecondName { get; init; }

        public static UniversitySystemUser Create(UniversitySystemUserCreateArguments createArguments)
        {
            return new UniversitySystemUser
            {
                Id = createArguments.Id ?? 0,
                FirstName = createArguments.FirstName,
                MiddleName = createArguments.MiddleName,
                SecondName = createArguments.SecondName
            };
        }
    }
}