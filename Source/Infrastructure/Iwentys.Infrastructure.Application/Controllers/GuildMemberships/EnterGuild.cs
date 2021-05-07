using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildMemberships
{
    public class EnterGuild
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }

            public Query(AuthorizedUser user, int guildId)
            {
                User = user;
                GuildId = guildId;
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
                Guild guild = await _guildRepository.GetById(request.GuildId);
                IwentysUser user = await _userRepository.GetById(request.User.Id);
                GuildLastLeave lastLeave = await _guildLastLeaveRepository.FindByIdAsync(user.Id);
                GuildMember guildMember = _guildMemberRepository
                    .Get()
                    .FirstOrDefault(m => m.Member.Id == request.User.Id && m.MemberType == GuildMemberType.Requested);

                GuildMember newMembership = guild.EnterGuild(user, guildMember, lastLeave);

                _guildMemberRepository.Insert(newMembership);
                await _unitOfWork.CommitAsync();
                return new Response();
            }
        }
    }
}