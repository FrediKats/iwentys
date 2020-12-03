using System.Collections.Generic;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Voting.ViewModels
{
    public class VotingAnswerStatisticDto
    {
        public int VariantId { get; set; }
        public string VariantText { get; set; }

        public List<StudentEntity> Students { get; set; }
    }
}