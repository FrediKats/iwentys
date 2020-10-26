using System;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities.Votes
{
    public class Voting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public VotingType VotingType { get; set; }
        public bool IsFinished { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime DueTo { get; set; }
    }
}