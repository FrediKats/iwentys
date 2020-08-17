using System;
using System.Linq;
using Iwentys.Models.Entities;

namespace Iwentys.Models.Types
{
    public enum QuestState
    {
        Active = 1,
        Completed = 2,
        Outdated = 3
    }

    public static class QuestStateExtensions
    {
        public static IQueryable<Quest> WhereIsNotOutdated(this IQueryable<Quest> query)
        {
            return query.Where(q => q.State == QuestState.Active && q.Deadline < DateTime.UtcNow);
        }
    }
}