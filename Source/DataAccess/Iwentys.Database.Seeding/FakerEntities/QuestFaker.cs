using System;
using Bogus;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class QuestFaker
    {
        public static readonly QuestFaker Instance = new QuestFaker();

        public Faker<Quest> CreateQuestFaker(int authorId)
        {
            return new Faker<Quest>()
                .RuleFor(q => q.Id, f => f.IndexFaker + 1)
                .RuleFor(q => q.Title, f => f.Lorem.Slug())
                .RuleFor(q => q.Description, f => f.Lorem.Paragraph())
                .RuleFor(q => q.CreationTime, DateTime.UtcNow)
                .RuleFor(q => q.Deadline, DateTime.UtcNow.AddDays(100))
                .RuleFor(q => q.State, QuestState.Active)
                .RuleFor(q => q.Price, 100)
                .RuleFor(q => q.AuthorId, authorId);
        }

        public CreateQuestRequest CreateQuestRequest(int price)
        {
            return new CreateQuestRequest(
                "Some quest",
                "Some desc",
                price,
                DateTime.UtcNow.AddDays(1));
        }
    }
}