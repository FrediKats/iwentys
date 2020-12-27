using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tournaments.Entities;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Domain
{
    public class CodeMarathonTournament : ITournamentDomain
    {
        private readonly TournamentEntity _tournament;
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IUnitOfWork _unitOfWork;

        public CodeMarathonTournament(TournamentEntity tournament, GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork)
        {
            _tournament = tournament;
            _githubIntegrationService = githubIntegrationService;
            _unitOfWork = unitOfWork;
        }

        public TournamentLeaderboardDto GetLeaderboard()
        {
            List<GuildEntity> guilds = _unitOfWork
                .GetRepository<GuildEntity>()
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

        private int CountGuildRating(GuildEntity guild)
        {
            var domain = new GuildDomain(guild, _githubIntegrationService, _unitOfWork.GetRepository<StudentEntity>(), _unitOfWork.GetRepository<GuildMemberEntity>());
            //TODO: remove result
            List<GuildMemberImpactDto> users = domain.GetMemberImpacts().Result;
            
            return users
                .Select(userData => userData.Contribution.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime))
                .Sum();
        }
    }
}