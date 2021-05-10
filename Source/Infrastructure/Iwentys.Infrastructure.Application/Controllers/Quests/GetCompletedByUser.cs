using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Quests;
using Iwentys.Domain.Quests.Dto;
using Iwentys.Infrastructure.Application.Controllers.Services;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Quests
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

                List<QuestInfoDto> result = await _context.Quests
                    .Where(Quest.IsCompletedBy(request.AuthorizedUser))
                    .Select(QuestInfoDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}