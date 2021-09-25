using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Quests;
using Iwentys.Domain.Quests.Dto;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Gamification.Quests.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.Gamification.Quests.Queries
{
    public class Complete
    {
        public class Query : IRequest<Response>
        {
            public int QuestId { get; set; }
            public AuthorizedUser AuthorizedUser { get; set; }
            public QuestCompleteArguments Arguments { get; set; }

            public Query(int questId, AuthorizedUser authorizedUser, QuestCompleteArguments arguments)
            {
                QuestId = questId;
                AuthorizedUser = authorizedUser;
                Arguments = arguments;
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
                Quest quest = await _context.Quests.GetById(request.QuestId);
                IwentysUser executor = await _context.IwentysUsers.GetById(request.Arguments.UserId);
                IwentysUser student = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                quest.MakeCompleted(student, executor, request.Arguments);

                _context.Quests.Update(quest);
                BarsPointTransaction transaction = BarsPointTransaction.ReceiveFromSystem(executor, quest.Price);

                _context.BarsPointTransactionLogs.Add(transaction);
                _context.IwentysUsers.Update(executor);
                _achievementProvider.AchieveForStudent(AchievementList.QuestComplete, executor.Id);
                await AchievementHack.ProcessAchievement(_achievementProvider, _context);
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