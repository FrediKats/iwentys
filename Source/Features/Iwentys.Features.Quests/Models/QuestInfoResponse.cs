using System;
using Iwentys.Common.Tools;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Quests.Models
{
    public class QuestInfoResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Deadline { get; set; }
        public QuestState State { get; set; }
        public bool IsOutdated { get; set; }

        public StudentInfoDto Author { get; set; }

        public static QuestInfoResponse Wrap(QuestEntity questEntity)
        {
            return new QuestInfoResponse
            {
                Id = questEntity.Id,
                Title = questEntity.Title,
                Description = questEntity.Description,
                Price = questEntity.Price,
                CreationTime = questEntity.CreationTime,
                Deadline = questEntity.Deadline,
                State = questEntity.State,
                IsOutdated = questEntity.Deadline < DateTime.UtcNow,
                Author = questEntity.Author.To(a => new StudentInfoDto(a))
            };
        }
    }
}