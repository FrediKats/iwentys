using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Infrastructure.Application.Repositories;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildMemberships
{
    public class RejectGuildRequest
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }
            public int MemberId { get; set; }

            public Query(AuthorizedUser user, int guildId, int memberId)
            {
                User = user;
                GuildId = guildId;
                MemberId = memberId;
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
                IwentysUser initiator = await _userRepository.GetById(request.User.Id);
                Guild guild = await _guildRepository.GetById(request.GuildId);
                IwentysUser iwentysUser = await _userRepository.GetById(request.MemberId);
                GuildLastLeave guildLastLeave = await GuildRepository.Get(iwentysUser, _guildLastLeaveRepository);

                GuildMember member = guild.Members.Find(m => m.MemberId == request.MemberId);

                if (member is null || member.MemberType != GuildMemberType.Requested)
                    throw InnerLogicException.GuildExceptions.RequestWasNotFound(request.MemberId, request.GuildId);

                guild.RemoveMember(initiator, iwentysUser, guildLastLeave);
                await _unitOfWork.CommitAsync();
                return new Response();
            }
        }
    }
}