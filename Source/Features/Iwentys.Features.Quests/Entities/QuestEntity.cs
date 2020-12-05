using System;
using System.Collections.Generic;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Quests.Entities
{
    public class QuestEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Deadline { get; set; }
        public QuestState State { get; set; }

        public int AuthorId { get; set; }
        public StudentEntity Author { get; set; }

        public List<QuestResponseEntity> Responses { get; set; }

        public bool IsOutdated => Deadline < DateTime.UtcNow;

        public static QuestEntity New(string title, string description, int price, DateTime? deadline, StudentEntity author)
        {
            return new QuestEntity
            {
                Title = title,
                Description = description,
                Price = price,
                CreationTime = DateTime.UtcNow,
                Deadline = deadline,
                State = QuestState.Active,
                AuthorId = author.Id
            };
        }
    }
}