using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Gamification
{
    public static class GetQuestById
    {
        public class Query : IRequest<Response>
        {
            public int QuestId { get; set; }
            public AuthorizedUser AuthorizedUser { get; set; }

            public Query(int questId, AuthorizedUser authorizedUser)
            {
                QuestId = questId;
                AuthorizedUser = authorizedUser;
            }
        }

        public class Response
        {
            public Response(QuestInfoDto questInfo)
            {
                QuestInfo = questInfo;
            }

            public QuestInfoDto QuestInfo { get; set; }
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
                QuestInfoDto result = await _context
                    .Quests
                    .Where(q => q.Id == request.QuestId)
                    .Select(QuestInfoDto.FromEntity)
                    .FirstAsync();

                return new Response(result);
            }
        }
    }
}