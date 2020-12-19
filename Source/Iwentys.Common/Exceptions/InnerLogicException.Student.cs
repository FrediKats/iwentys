using System.Globalization;

namespace Iwentys.Common.Exceptions
{
    public partial class InnerLogicException
    {
        public static class Student
        {
            public static InnerLogicException GithubAlreadyUser(string githubUsername)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, ExceptionMessages.StudentExceptionMessages.GithubAlreadyUser, githubUsername));
            }
        }
    }
}
