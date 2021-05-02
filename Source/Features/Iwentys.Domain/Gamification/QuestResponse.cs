using System;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended.Models;

namespace Iwentys.Domain.Gamification
{
    public class QuestResponse
    {
        public int QuestId { get; init; }
        public virtual Quest Quest { get; init; }

        public int StudentId { get; init; }
        public virtual IwentysUser Student { get; init; }

        public DateTime ResponseTime { get; init; }
        public string Description { get; set; }

        public static QuestResponse New(int questId, IwentysUser responseCreator, QuestResponseCreateArguments arguments)
        {
            return new QuestResponse
            {
                QuestId = questId,
                StudentId = responseCreator.Id,
                ResponseTime = DateTime.UtcNow,
                Description = arguments.Description
            };
        }
    }
}