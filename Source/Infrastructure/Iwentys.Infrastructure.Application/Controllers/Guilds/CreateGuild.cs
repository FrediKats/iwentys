using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.Controllers.Services;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Guilds
{
    public static class CreateGuild
    {
        public class Query : IRequest<Response>
        {
            public GuildCreateRequestDto Arguments { get; set; }
            public AuthorizedUser AuthorizedUser { get; set; }

            public Query(GuildCreateRequestDto arguments, AuthorizedUser authorizedUser)
            {
                Arguments = arguments;
                AuthorizedUser = authorizedUser;
            }
        }

        public class Response
        {
            public Response(GuildProfileShortInfoDto guild)
            {
                Guild = guild;
            }

            public GuildProfileShortInfoDto Guild { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<GuildMember> _guildMemberRepository;
            private readonly IGenericRepository<Guild> _guildRepository;
            private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;

            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepository = _unitOfWork.GetRepository<Guild>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser creator = _iwentysUserRepository.GetById(request.AuthorizedUser.Id).Result;
                Guild userCurrentGuild = _guildMemberRepository.ReadForStudent(creator.Id);

                var createdGuild = Guild.Create(creator, userCurrentGuild, request.Arguments);

                createdGuild = _guildRepository.Insert(createdGuild);
                await _unitOfWork.CommitAsync();
                return new Response(new GuildProfileShortInfoDto(createdGuild));
            }
        }
    }
}