using System;
using System.Collections.Generic;
using Iwentys.Models.Entities;

namespace Iwentys.Models.Transferable.Voting
{
    public class VotingInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DueTo { get; set; }

        public List<VotingAnswerStatisticDto> Answers { get; set; }
        public List<Student> WithoutVote { get; set; }
    }
}