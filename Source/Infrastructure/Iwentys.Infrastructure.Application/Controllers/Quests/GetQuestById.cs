using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Quests.Dto;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Quests
{
    public class GetQuestById
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
            private readonly AchievementProvider _achievementProvider;
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context, AchievementProvider achievementProvider)
            {
                _context = context;
                _achievementProvider = achievementProvider;
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