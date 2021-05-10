using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.AccountManagement.Dto;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Quests;
using Iwentys.Domain.Quests.Dto;
using Iwentys.Infrastructure.Application.Controllers.Services;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Quests
{
    public class GetQuestExecutorRating
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
            public Response(List<QuestRatingRow> questRatingRows)
            {
                QuestRatingRows = questRatingRows;
            }

            public List<QuestRatingRow> QuestRatingRows { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly AchievementProvider _achievementProvider;
            private readonly BarsPointTransactionLogService _pointTransactionLogService;
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context, BarsPointTransactionLogService pointTransactionLogService, AchievementProvider achievementProvider)
            {
                _context = context;
                _pointTransactionLogService = pointTransactionLogService;
                _achievementProvider = achievementProvider;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                List<QuestRatingRow> result = _context.Quests
                    .Where(q => q.State == QuestState.Completed)
                    .AsEnumerable()
                    .GroupBy(q => q.ExecutorId, q => q.ExecutorMark)
                    .Select(g => new QuestRatingRow { UserId = g.Key.Value, Marks = g.ToList() })
                    .ToList();

                List<IwentysUser> users = await _context.IwentysUsers.ToListAsync();
                //TODO: hack
                result.ForEach(r => { r.User = new IwentysUserInfoDto(users.First(u => u.Id == r.UserId)); });

                return new Response(result);
            }
        }
    }
}