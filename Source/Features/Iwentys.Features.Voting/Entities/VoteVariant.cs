namespace Iwentys.Features.Voting.Entities
{
    public class VoteVariant
    {
        public int VotingId { get; init; }
        public virtual Voting Voting { get; init; }

        public int VariantId { get; init; }
        public string VariantText { get; init; }
    }
}