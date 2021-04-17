using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Domain.Guilds
{
    public class CodeMarathonTournamentDomain : ITournamentDomain
    {
        private readonly AchievementProvider _achievementProvider;
        private readonly IGithubUserApiAccessor _githubUserApiAccessor;

        private readonly Tournament _tournament;

        private readonly IGenericRepository<TournamentTeamMember> _tournamentTeamMemberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CodeMarathonTournamentDomain(
            Tournament tournament,
            IUnitOfWork unitOfWork,
            AchievementProvider achievementProvider, IGithubUserApiAccessor githubUserApiAccessor)
        {
            _tournament = tournament;
            _unitOfWork = unitOfWork;
            _achievementProvider = achievementProvider;
            _githubUserApiAccessor = githubUserApiAccessor;

            _tournamentTeamMemberRepository = _unitOfWork.GetRepository<TournamentTeamMember>();
        }

        public TournamentLeaderboardDto GetLeaderboard()
        {
            List<Guild> guilds = _unitOfWork
                .GetRepository<Guild>()
                .Get()
                .ToList();

            Dictionary<GuildProfileShortInfoDto, int> result = guilds
                .ToDictionary(g => new GuildProfileShortInfoDto(g), CountGuildRating);

            return new TournamentLeaderboardDto
            {
                Tournament = _tournament,
                Result = result
            };
        }

        public async Task RewardWinners()
        {
            if (_tournament.IsActive)
                throw InnerLogicException.TournamentException.IsNotFinished(_tournament.Id);

            TournamentParticipantTeam winner = await _unitOfWork.GetRepository<TournamentParticipantTeam>()
                .Get()
                .Where(team => team.TournamentId == _tournament.Id)
                .OrderByDescending(t => t.Members.Sum(m => m.Points))
                .FirstOrDefaultAsync();

            if (winner is null)
                throw InnerLogicException.TournamentException.NoTeamRegistered(_tournament.Id);

            await _achievementProvider.AchieveForGuild(AchievementList.Tournaments.TournamentWinner, winner.GuildId);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateResult()
        {
            if (!_tournament.IsActive)
                throw InnerLogicException.TournamentException.AlreadyFinished(_tournament.Id);

            List<TournamentTeamMember> members = await _unitOfWork.GetRepository<TournamentParticipantTeam>()
                .Get()
                .Where(team => team.TournamentId == _tournament.Id)
                .SelectMany(team => team.Members)
                .Include(m => m.Member)
                .ToListAsync();

            foreach (TournamentTeamMember member in members)
            {
                ContributionFullInfo contributionFullInfo = await _githubUserApiAccessor.FindUserContributionOrEmpty(member.Member);
                member.Points = contributionFullInfo.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime);
            }

            _tournamentTeamMemberRepository.Update(members);
            await _unitOfWork.CommitAsync();
        }

        private int CountGuildRating(Guild guild)
        {
            var domain = new GuildDomain(guild,
                _unitOfWork.GetRepository<IwentysUser>(),
                _unitOfWork.GetRepository<GuildMember>(),
                _unitOfWork.GetRepository<GuildLastLeave>(),
                _githubUserApiAccessor);
            //TODO: remove result
            List<GuildMemberImpactDto> users = domain.GetMemberImpacts().Result;

            return users
                .Select(userData => userData.Contribution.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime))
                .Sum();
        }
    }
}