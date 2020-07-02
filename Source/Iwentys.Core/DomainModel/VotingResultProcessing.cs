using System;
using System.Linq;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Votes;
using Iwentys.Models.Types;

namespace Iwentys.Core.DomainModel
{
    public class VotingResultProcessing
    {
        private readonly IVotingRepository _votingRepository;

        public VotingResultProcessing(IVotingRepository votingRepository)
        {
            _votingRepository = votingRepository;
        }

        public void PostProcessForFinishedVotings()
        {
            foreach (Voting voting in _votingRepository.Read().Where(v => !v.IsFinished))
            {
                throw new NotImplementedException();

                switch (voting.VotingType)
                {
                    case VotingType.GuildCommon:
                        break;
                    case VotingType.GuildLeader:
                        break;
                    case VotingType.GuildTotem:
                        break;
                    case VotingType.GroupCommon:
                        break;
                    case VotingType.GroupAdmin:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void PostProcessGuildTotemVote(Voting voting)
        {
            throw new NotImplementedException();
        }
    }
}