using System;
using System.Collections.Generic;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Voting.ViewModels
{
    public class VotingInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DueTo { get; set; }

        public List<VotingAnswerStatisticDto> Answers { get; set; }
        public List<StudentEntity> WithoutVote { get; set; }
    }
}