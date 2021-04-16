namespace Iwentys.Domain
{
    public class GuildLeaderVotingAnswer
    {
        public int VotingId { get; init; }
        public virtual Voting Voting { get; init; }

        public int CandidateId { get; init; }
        public virtual IwentysUser Candidate { get; init; }

        public int StudentId { get; init; }
        public virtual IwentysUser Student { get; init; }
    }
}