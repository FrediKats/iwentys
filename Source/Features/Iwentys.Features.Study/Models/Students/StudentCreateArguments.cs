using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.Study.Enums;

namespace Iwentys.Features.Study.Models.Students
{
    public class StudentCreateArguments : IwentysUserCreateArguments
    {
        public StudentCreateArguments(int? id, string firstName, string middleName, string secondName, bool isAdmin, string githubUsername, int barsPoints, string avatarUrl, StudentType type, string group)
            : base(id, firstName, middleName, secondName, isAdmin, githubUsername, barsPoints, avatarUrl)
        {
            Type = type;
            Group = group;
        }

        public StudentCreateArguments()
        {
        }

        public StudentType Type { get; init; }
        public string Group { get; init; }
    }
}