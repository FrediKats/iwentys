using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Quests;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Gamification
{
    public class Create
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser AuthorizedUser { get; set; }
            public CreateQuestRequest Arguments { get; set; }

            public Query(AuthorizedUser authorizedUser, CreateQuestRequest arguments)
            {
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
                IwentysUser student = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);
                var quest = Quest.New(student, request.Arguments);

                _context.Quests.Add(quest);
                _context.IwentysUsers.Update(student);

                _achievementProvider.AchieveForStudent(AchievementList.QuestCreator, request.AuthorizedUser.Id);
                await AchievementHack.ProcessAchievement(_achievementProvider, _context);

                QuestInfoDto result = await _context
                    .Quests
                    .Where(q => q.Id == quest.Id)
                    .Select(QuestInfoDto.FromEntity)
                    .FirstAsync();

                return new Response(result);
            }
        }
    }
}