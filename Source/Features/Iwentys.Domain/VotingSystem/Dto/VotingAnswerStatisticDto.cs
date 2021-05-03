using System.Collections.Generic;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.VotingSystem.Dto
{
    public class VotingAnswerStatisticDto
    {
        public int VariantId { get; set; }
        public string VariantText { get; set; }

        public List<IwentysUser> Students { get; set; }
    }
}