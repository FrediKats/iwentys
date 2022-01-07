using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Quests;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Gamification.Quests.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.Gamification.Quests.Queries
{
    public class GetCompletedByUser
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
                IwentysUser user = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                List<QuestInfoDto> result = await _context.Quests
                    .Where(Quest.IsCompletedBy(user))
                    .Select(QuestInfoDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}