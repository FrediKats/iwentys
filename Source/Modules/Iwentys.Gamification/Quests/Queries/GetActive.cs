﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.Quests;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Gamification
{
    public class GetActive
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser AuthorizedUser { get; set; }

            public Query(AuthorizedUser authorizedUser)
            {
                AuthorizedUser = authorizedUser;
            }
        }

        public class Response
        {
            public Response(List<QuestInfoDto> questInfos)
            {
                QuestInfos = questInfos;
            }

            public List<QuestInfoDto> QuestInfos { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                List<QuestInfoDto> result = await _context.Quests
                    .Where(Quest.IsActive)
                    .Select(QuestInfoDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}