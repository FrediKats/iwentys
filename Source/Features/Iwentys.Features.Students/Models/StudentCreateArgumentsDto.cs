using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Students.Models
{
    public record StudentCreateArgumentsDto(
        int Id,
        string FirstName,
        string MiddleName,
        string SecondName,
        UserType Role,
        string Group,
        string GithubUsername,
        int BarsPoints);
}