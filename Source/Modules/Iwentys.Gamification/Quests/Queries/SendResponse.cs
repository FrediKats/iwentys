﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Quests;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Gamification;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, AchievementProvider achievementProvider, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _achievementProvider = achievementProvider;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser student = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);

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