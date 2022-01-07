﻿using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.VotingSystem
{
    public class VoteAnswer
    {
        public int VotingId { get; init; }
        public virtual Voting Voting { get; init; }

        public IwentysUser Student { get; init; }
        public int StudentId { get; init; }
        public int SelectedVariant { get; init; }
    }
}