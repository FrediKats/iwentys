namespace Iwentys.Models.Entities.Votes
{
    public class VoteAnswer
    {
        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public UserProfile UserProfile { get; set; }
        public int UserProfileId { get; set; }
        public int SelectedVariant { get; set; }
    }
}