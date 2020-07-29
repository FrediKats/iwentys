using System;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities.Assignments
{
    public class StudentAssignment
    {
        public Int32 StudentId { get; set; }
        public Int32 AssignmentId { get; set; }
        public AssignmentStatus Status { get; set; }
        public DateTime DeadLine { get; set; }
    }
}