using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using Iwentys.Features.Guilds.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Tournaments
{
    public class RegisterToTournament
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, int tournamentId)
            {
                User = user;
                TournamentId = tournamentId;
            }

            public AuthorizedUser User { get; set; }
            public int TournamentId { get; set; }
            public CreateCodeMarathonTournamentArguments Arguments { get; set; }
        }

        public class Response
        {
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly AchievementProvider _achievementProvider;
            private readonly IGenericRepository<CodeMarathonTournament> _codeMarathonTournamentRepository;
            private readonly GithubIntegrationService _githubIntegrationService;
            private readonly IGenericRepository<GuildMember> _guildMemberRepository;
            private readonly IGenericRepository<Guild> _guildRepository;

            private readonly IGenericRepository<IwentysUser> _studentRepository;
            private readonly IGenericRepository<Tournament> _tournamentRepository;
            private readonly IGenericRepository<TournamentParticipantTeam> _tournamentTeamRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService, AchievementProvider achievementProvider)
            {
                _unitOfWork = unitOfWork;
                _achievementProvider = achievementProvider;

                _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepository = _unitOfWork.GetRepository<Guild>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
                _tournamentRepository = _unitOfWork.GetRepository<Tournament>();
                _tournamentTeamRepository = _unitOfWork.GetRepository<TournamentParticipantTeam>();
                _codeMarathonTournamentRepository = _unitOfWork.GetRepository<CodeMarathonTournament>();
                _githubIntegrationService = githubIntegrationService;
            }

            protected override Response Handle(Query request)
            {
                IwentysUser studentEntity = _studentRepository.GetById(request.User.Id).Result;
                Guild guild = _guildMemberRepository.ReadForStudent(request.User.Id);
                //TODO: check guild for null
                GuildMentor guildMentorUser = studentEntity.EnsureIsGuildMentor(guild);
                Tournament tournamentEntity = _tournamentRepository.GetById(request.TournamentId).Result;
                List<GuildMember> members = _guildRepository
                    .Get()
                    .SelectMany(g => g.Members)
                    .ToList();

                TournamentParticipantTeam tournamentParticipantTeamEntity = tournamentEntity.RegisterTeam(guildMentorUser.Guild, members);

                _tournamentTeamRepository.InsertAsync(tournamentParticipantTeamEntity).Wait();
                _unitOfWork.CommitAsync().Wait();
                return new Response();
            }
        }
    }
}