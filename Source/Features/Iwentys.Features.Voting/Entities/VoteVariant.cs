namespace Iwentys.Features.Voting.Entities
{
    public class VoteVariant
    {
        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public int VariantId { get; set; }
        public string VariantText { get; set; }
    }
}