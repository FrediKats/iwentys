using System;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Quests.Models;

namespace Iwentys.Features.Quests.Entities
{
    public class QuestResponse
    {
        public int QuestId { get; init; }
        public virtual Quest Quest { get; init; }

        public int StudentId { get; init; }
        public virtual IwentysUser Student { get; init; }

        public DateTime ResponseTime { get; init; }
        public string Description { get; set; }

        public static QuestResponse New(int questId, AuthorizedUser responseCreator, QuestResponseCreateArguments arguments)
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