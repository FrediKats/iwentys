using System.Globalization;

namespace Iwentys.Common;

public partial class InnerLogicException
{
    public static class StudentExceptions
    {
        public static InnerLogicException GithubAlreadyUser(string githubUsername)
        {
            return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, StudentExceptionMessages.GithubAlreadyUser, githubUsername));
        }
    }
}