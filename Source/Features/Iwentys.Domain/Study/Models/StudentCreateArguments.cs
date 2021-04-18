using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study.Enums;

namespace Iwentys.Domain.Study.Models
{
    public class StudentCreateArguments : IwentysUserCreateArguments
    {
        public StudentCreateArguments(int? id, string firstName, string middleName, string secondName, bool isAdmin, string githubUsername, int barsPoints, string avatarUrl, StudentType type, string group, int groupId)
            : base(id, firstName, middleName, secondName, isAdmin, githubUsername, barsPoints, avatarUrl)
        {
            Type = type;
            Group = group;
            GroupId = groupId;
        }

        public StudentCreateArguments()
        {
        }

        public StudentType Type { get; init; }
        public string Group { get; init; }
        public int GroupId { get; set; }
    }
}