using System.Collections.Generic;
using Iwentys.Infrastructure.Application.Controllers.Study.Dtos;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments.Dtos
{
    public class SubjectAssignmentJournalItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<SubjectAssignmentDto> Assignments { get; set; }
    }
}