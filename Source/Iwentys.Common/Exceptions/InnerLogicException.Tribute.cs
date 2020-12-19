using System.Globalization;
using Iwentys.Common.ExceptionMessages;

namespace Iwentys.Common.Exceptions
{
    public partial class InnerLogicException
    {
        public static class Tribute
        {
            public static InnerLogicException ProjectAlreadyUsed(long projectId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, TributeExceptionMessages.ProjectAlreadyUsed, projectId));
            }

            public static InnerLogicException UserAlreadyHaveTribute(int userId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, TributeExceptionMessages.UserAlreadyHaveTribute, userId));
            }

            public static InnerLogicException IsNotActive(long projectId)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, TributeExceptionMessages.IsNotActive, projectId));
            }

            public static InnerLogicException TributeCanBeSendFromStudentAccount(int studentId, string projectOwner)
            {
                return new InnerLogicException(string.Format(CultureInfo.InvariantCulture, TributeExceptionMessages.TributeCanBeSendFromStudentAccount, studentId, projectOwner));
            }
        }
    }
}
