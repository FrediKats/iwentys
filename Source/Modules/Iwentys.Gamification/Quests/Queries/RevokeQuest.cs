using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Quests;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Gamification
{
    public class RevokeQuest
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

                IwentysUser author = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);
                Quest quest = await _context.Quests.GetById(request.QuestId);

                quest.Revoke(author);

                _context.IwentysUsers.Update(author);
                _context.Quests.Update(quest);

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