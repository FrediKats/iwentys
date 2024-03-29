﻿using System.Collections.Generic;
using System.Linq;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.Guilds;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public class GetPendingTributes
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser user)
        {
            User = user;
        }

        public AuthorizedUser User { get; set; }
    }

    public class Response
    {
        public Response(List<TributeInfoResponse> tributes)
        {
            Tributes = tributes;
        }

        public List<TributeInfoResponse> Tributes { get; set; }
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
            Guild guild = _context.GuildMembers.ReadForStudent(request.User.Id).Result ?? throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, null);

            List<TributeInfoResponse> result = _context
                .Tributes
                .Where(t => t.GuildId == guild.Id)
                .Where(t => t.Project.OwnerUserId == request.User.Id)
                .Select(TributeInfoResponse.FromEntity)
                .ToList();

            return new Response(result);
        }
    }
}