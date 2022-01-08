using System.Collections.Generic;
using System.Linq;
using Iwentys.DataAccess;
using Iwentys.Domain.Guilds;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public class GetGuildTributes
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser user, int guildId)
        {
            User = user;
            GuildId = guildId;
        }

        public AuthorizedUser User { get; set; }
        public int GuildId { get; set; }
    }

    public class Response
    {
        public Response(List<TributeInfoResponse> tribute)
        {
            Tribute = tribute;
        }

        public List<TributeInfoResponse> Tribute { get; set; }
    }

    public class Handler : RequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;

        public Handler(IwentysDbContext context)
        {
            _context = context;
        }

        protected override Response Handle(Query request)
        {
            List<TributeInfoResponse> responses = _context
                .Tributes
                .Where(t => t.GuildId == request.GuildId)
                .Where(t => t.State == TributeState.Active)
                .Select(TributeInfoResponse.FromEntity)
                .ToList();

            return new Response(responses);
        }
    }
}