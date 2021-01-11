using Iwentys.Features.AccountManagement.Models;

namespace Iwentys.Features.Study.Models.Students
{
    public class StudentCreateArguments : IwentysUserCreateArguments
    {
        public StudentCreateArguments(int? id, string firstName, string middleName, string secondName, bool isAdmin, string githubUsername, int barsPoints, string avatarUrl, string @group)
            : base(id, firstName, middleName, secondName, isAdmin, githubUsername, barsPoints, avatarUrl)
        {
            Group = @group;
        }

        public string Group { get; init; }
    }
}