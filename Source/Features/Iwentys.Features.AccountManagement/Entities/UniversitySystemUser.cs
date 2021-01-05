using System.ComponentModel.DataAnnotations.Schema;

namespace Iwentys.Features.AccountManagement.Entities
{
    public class UniversitySystemUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; init; }

        public string FirstName { get; init; }
        public string MiddleName { get; init; }
        public string SecondName { get; init; }
    }
}