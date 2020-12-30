using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Voting.Entities
{
    public class VoteAnswer
    {
        public int VotingId { get; init; }
        public virtual Voting Voting { get; init; }

        public Student Student { get; init; }
        public int StudentId { get; init; }
        public int SelectedVariant { get; init; }
    }
}