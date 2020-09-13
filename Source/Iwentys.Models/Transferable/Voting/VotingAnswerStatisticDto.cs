using System.Collections.Generic;
using Iwentys.Models.Entities;

namespace Iwentys.Models.Transferable.Voting
{
    public class VotingAnswerStatisticDto
    {
        public int VariantId { get; set; }
        public string VariantText { get; set; }

        public List<StudentEntity> Students { get; set; }
    }
}