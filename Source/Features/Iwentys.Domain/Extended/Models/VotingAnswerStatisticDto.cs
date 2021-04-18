using System.Collections.Generic;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Extended.Models
{
    public class VotingAnswerStatisticDto
    {
        public int VariantId { get; set; }
        public string VariantText { get; set; }

        public List<IwentysUser> Students { get; set; }
    }
}