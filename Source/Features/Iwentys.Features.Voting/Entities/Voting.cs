using System;
using Iwentys.Features.Voting.Enums;

namespace Iwentys.Features.Voting.Entities
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