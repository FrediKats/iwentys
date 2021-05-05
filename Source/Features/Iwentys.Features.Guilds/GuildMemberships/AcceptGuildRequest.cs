using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using MediatR;

namespace Iwentys.Features.Guilds.GuildMemberships
{
    public class AcceptGuildRequest
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }
            public int MemberForAccepting { get; set; }

            public Query(AuthorizedUser user, int guildId, int memberForAccepting)
            {
                User = user;
                GuildId = guildId;
                MemberForAccepting = memberForAccepting;
            }
        }

        public class Response
        {

        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<GuildMember> _guildMemberRepository;
            private readonly IGenericRepository<Guild> _guildRepository;
            private readonly IGenericRepository<GuildLastLeave> _guildLastLeaveRepository;
            private readonly IGenericRepository<IwentysUser> _userRepository;

            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _userRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepository = _unitOfWork.GetRepository<Guild>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
                _guildLastLeaveRepository = _unitOfWork.GetRepository<GuildLastLeave>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser student = await _userRepository.GetById(request.User.Id);
                Guild guild = await _guildRepository.GetById(request.GuildId);
                GuildMember member = guild.Members.Find(m => m.MemberId == request.MemberForAccepting) ?? throw EntityNotFoundException.Create(typeof(GuildMember), request.MemberForAccepting);

                GuildMentor guildMentor = student.EnsureIsGuildMentor(guild);
                member.Approve(guildMentor);

                _guildMemberRepository.Update(member);
                await _unitOfWork.CommitAsync();
                return new Response();
            }
        }
    }
}