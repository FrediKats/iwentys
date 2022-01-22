using System.ComponentModel.DataAnnotations.Schema;

namespace Iwentys.EntityManager.Domain;

public class UniversitySystemUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string FirstName { get; init; }
    public string MiddleName { get; init; }
    public string SecondName { get; init; }
}