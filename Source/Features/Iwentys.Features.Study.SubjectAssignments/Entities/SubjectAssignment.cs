using System.Collections.Generic;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.SubjectAssignments.Entities
{
    public class SubjectAssignment
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        //TODO: add deadline etc?

        public virtual ICollection<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }
    }
}