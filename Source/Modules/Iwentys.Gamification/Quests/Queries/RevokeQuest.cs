using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Quests;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Gamification;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {

            IwentysUser author = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);
            Quest quest = await _context.Quests.GetById(request.QuestId);

            quest.Revoke(author);

            _entityManagerApiClient.IwentysUserProfiles.Update(author);
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