using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Voting.Entities
{
    public class VoteAnswer
    {
        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public Student Student { get; set; }
        public int StudentId { get; set; }
        public int SelectedVariant { get; set; }
    }
}