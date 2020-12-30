using System.Collections.Generic;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Voting.Models
{
    public class VotingAnswerStatisticDto
    {
        public int VariantId { get; set; }
        public string VariantText { get; set; }

        public List<Student> Students { get; set; }
    }
}