using System.Collections.Generic;
using Iwentys.Features.AccountManagement.Entities;

namespace Iwentys.Features.Voting.Models
{
    public class VotingAnswerStatisticDto
    {
        public int VariantId { get; set; }
        public string VariantText { get; set; }

        public List<IwentysUser> Students { get; set; }
    }
}