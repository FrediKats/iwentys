using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application.Controllers.Services;
using Iwentys.Infrastructure.Application.Repositories;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildMemberships
{
    public class LeaveGuild
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
                IwentysUser iwentysUser = await _userRepository.GetById(request.User.Id);
                GuildLastLeave guildLastLeave = await GuildRepository.Get(iwentysUser, _guildLastLeaveRepository);

                Guild studentGuild = _guildMemberRepository.ReadForStudent(request.User.Id);
                if (studentGuild is null || studentGuild.Id != request.GuildId)
                    throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, request.GuildId);

                //TributeEntity userTribute = _guildTributeRepository.Get()
                //    .Where(t => t.GuildId == guildId)
                //    .Where(t => t.ProjectEntity.AuthorId == user.Id)
                //    .SingleOrDefault(t => t.State == TributeState.Active);

                //if (userTribute is not null)
                //    await _guildTributeRepository.DeleteAsync(userTribute.ProjectId);

                studentGuild.RemoveMember(iwentysUser, iwentysUser, guildLastLeave);
                await _unitOfWork.CommitAsync();
                return new Response();
            }
        }
    }
}