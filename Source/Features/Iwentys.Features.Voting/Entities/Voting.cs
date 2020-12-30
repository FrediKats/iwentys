using System;
using Iwentys.Features.Voting.Enums;

namespace Iwentys.Features.Voting.Entities
{
    public class Voting
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public VotingType VotingType { get; init; }
        public bool IsFinished { get; init; }

        public DateTime StartTime { get; init; }
        public DateTime DueTo { get; init; }
    }
}