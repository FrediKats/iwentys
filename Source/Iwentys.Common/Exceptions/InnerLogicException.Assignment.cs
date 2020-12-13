using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iwentys.Common.ExceptionMessages;

namespace Iwentys.Common.Exceptions
{
    public partial class InnerLogicException
    {
        public static class Assignment
        {
            public static InnerLogicException IsNotAssignmentCreator(int assignmentId, int userId)
            {
                return new InnerLogicException($"Assignment {assignmentId} doesn't belong to this user {userId}");
            }
        } 
    }
}
