using System;
using Bogus;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Enums;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class QuestFaker : Faker<Quest>
    {
        public QuestFaker(int authorId)
        {
            this
                .RuleFor(q => q.Id, f => f.IndexFaker + 1)
                .RuleFor(q => q.Title, f => f.Lorem.Slug())
                .RuleFor(q => q.Description, f => f.Lorem.Paragraph())
                .RuleFor(q => q.CreationTime, DateTime.UtcNow)
                .RuleFor(q => q.Deadline, DateTime.UtcNow.AddDays(100))
                .RuleFor(q => q.State, QuestState.Active)
                .RuleFor(q => q.Price, 100)
                .RuleFor(q => q.AuthorId, authorId);
        }
    }
}