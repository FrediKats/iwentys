namespace Iwentys.Models.Entities.Votes
{
    public class VoteAnswer
    {
        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public StudentEntity Student { get; set; }
        public int StudentId { get; set; }
        public int SelectedVariant { get; set; }
    }
}