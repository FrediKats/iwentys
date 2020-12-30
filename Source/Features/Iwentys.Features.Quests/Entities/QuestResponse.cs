using System;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Quests.Entities
{
    public class QuestResponse
    {
        public int QuestId { get; set; }
        public virtual Quest Quest { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public DateTime ResponseTime { get; set; }

        public static QuestResponse New(int questId, AuthorizedUser responseCreator)
        {
            return new QuestResponse
            {
                QuestId = questId,
                StudentId = responseCreator.Id,
                ResponseTime = DateTime.UtcNow
            };
        }
    }
}