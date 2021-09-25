using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
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
    public class SendResponse
    {
        public class Query : IRequest<Response>
        {
            public int QuestId { get; set; }
            public AuthorizedUser AuthorizedUser { get; set; }
            public QuestResponseCreateArguments Arguments { get; set; }

            public Query(int questId, AuthorizedUser authorizedUser, QuestResponseCreateArguments arguments)
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
                IwentysUser student = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                Quest quest = await _context.Quests.GetById(request.QuestId);

                QuestResponse questResponseEntity = quest.CreateResponse(student, request.Arguments);

                _context.QuestResponses.Add(questResponseEntity);

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