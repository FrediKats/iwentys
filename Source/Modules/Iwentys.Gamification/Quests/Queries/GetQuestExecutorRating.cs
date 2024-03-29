﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.AccountManagement;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Quests;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Gamification;

public static class GetQuestExecutorRating
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
        private readonly IwentysDbContext _context;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            List<QuestRatingRow> result = _context.Quests
                .Where(q => q.State == QuestState.Completed)
                .AsEnumerable()
                .GroupBy(q => q.ExecutorId, q => q.ExecutorMark)
                .Select(g => new QuestRatingRow { UserId = g.Key.Value, Marks = g.ToList() })
                .ToList();

            IReadOnlyCollection<IwentysUser> users = await _entityManagerApiClient.IwentysUserProfiles.GetAsync();
            //TODO: hack
            result.ForEach(r => { r.User = EntityManagerApiDtoMapper.Map(users.First(u => u.Id == r.UserId)); });

            return new Response(result);
        }
    }
}