using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.Controllers.Services;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Tournaments
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
            private readonly IGenericRepository<GuildMember> _guildMemberRepository;

            private readonly IGenericRepository<IwentysUser> _studentRepository;
            private readonly IGenericRepository<Tournament> _tournamentRepository;
            private readonly IGenericRepository<TournamentParticipantTeam> _tournamentTeamRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
                _tournamentRepository = _unitOfWork.GetRepository<Tournament>();
                _tournamentTeamRepository = _unitOfWork.GetRepository<TournamentParticipantTeam>();
            }

            protected override Response Handle(Query request)
            {
                IwentysUser studentEntity = _studentRepository.GetById(request.User.Id).Result;
                Guild guild = _guildMemberRepository.ReadForStudent(request.User.Id);
                Tournament tournamentEntity = _tournamentRepository.GetById(request.TournamentId).Result;

                TournamentParticipantTeam tournamentParticipantTeamEntity = tournamentEntity.RegisterTeam(studentEntity, guild);

                _tournamentTeamRepository.Insert(tournamentParticipantTeamEntity);
                _unitOfWork.CommitAsync().Wait();
                return new Response();
            }
        }
    }
}