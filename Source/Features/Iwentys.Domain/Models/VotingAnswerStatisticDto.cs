using System.Collections.Generic;

namespace Iwentys.Domain.Models
{
    public class VotingAnswerStatisticDto
    {
        public int VariantId { get; set; }
        public string VariantText { get; set; }

        public List<IwentysUser> Students { get; set; }
    }
}