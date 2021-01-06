using System;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Tests.TestCaseContexts
{
    public class QuestTestCaseContext
    {
        private readonly TestCaseContext _context;

        public QuestTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public QuestInfoDto WithQuest(AuthorizedUser user, int price)
        {
            var request = new CreateQuestRequest(
                "Some quest",
                "Some desc",
                price,
                DateTime.UtcNow.AddDays(1));

            QuestInfoDto quest = _context.QuestService.Create(user, request).Result;

            return quest;
        }
    }
}