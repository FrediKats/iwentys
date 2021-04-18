using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using MediatR;

namespace Iwentys.Features.Guilds.Guilds
{
    public class UpdateGuild
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, GuildUpdateRequestDto arguments)
            {
                AuthorizedUser = authorizedUser;
                Arguments = arguments;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
            public GuildUpdateRequestDto Arguments { get; set; }
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
            private readonly IGenericRepository<Guild> _guildRepository;
            private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;

            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepository = _unitOfWork.GetRepository<Guild>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Guild guild = await _guildRepository.GetById(request.Arguments.Id);
                IwentysUser user = await _iwentysUserRepository.GetById(request.AuthorizedUser.Id);

                guild.Update(user, request.Arguments);

                _guildRepository.Update(guild);
                await _unitOfWork.CommitAsync();
                return new Response(new GuildProfileShortInfoDto(guild));
            }
        }
    }
}