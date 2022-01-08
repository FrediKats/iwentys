using System.Globalization;

namespace Iwentys.Common;

public partial class InnerLogicException
{
    public static class TributeExceptions
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