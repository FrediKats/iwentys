using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Guilds
{
    public class GetGuildById
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
            public Response(GuildProfileDto guild)
            {
                Guild = guild;
            }

            public GuildProfileDto Guild { get; set; }
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
                GuildProfileDto guildProfileDto = await _guildRepository
                    .Get()
                    .Where(g => g.Id == request.GuildId)
                    .Select(GuildProfileDto.FromEntity)
                    .SingleAsync(cancellationToken);

                return new Response(guildProfileDto);
            }
        }
    }
}