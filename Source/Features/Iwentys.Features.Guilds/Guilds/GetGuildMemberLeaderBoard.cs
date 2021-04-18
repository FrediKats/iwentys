using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using MediatR;

namespace Iwentys.Features.Guilds.Guilds
{
    public class GetGuildMemberLeaderBoard
    {
        public class Query : IRequest<Response>
        {
            public Query(int guildId)
            {
                GuildId = guildId;
            }

            public int GuildId { get; set; }
        }

        public class Response
        {
            public Response(GuildMemberLeaderBoardDto guildMemberLeaderBoard)
            {
                GuildMemberLeaderBoard = guildMemberLeaderBoard;
            }

            public GuildMemberLeaderBoardDto GuildMemberLeaderBoard { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<Guild> _guildRepository;

            public Handler(IUnitOfWork unitOfWork)
            {
                _guildRepository = unitOfWork.GetRepository<Guild>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Guild guild = await _guildRepository.GetById(request.GuildId);

                return new Response(new GuildMemberLeaderBoardDto(guild.GetImpact()));
            }
        }
    }
}