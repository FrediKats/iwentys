using System;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;
using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable.Gamification
{
    public class QuestInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Deadline { get; set; }
        public QuestState State { get; set; }
        public Boolean IsOutdated { get; set; }

        public StudentPartialProfileDto Author { get; set; }

        public static QuestInfoDto Wrap(Quest quest)
        {
            return new QuestInfoDto
            {
                Id = quest.Id,
                Title = quest.Title,
                Description = quest.Description,
                Price = quest.Price,
                CreationTime = quest.CreationTime,
                Deadline = quest.Deadline,
                State = quest.State,
                IsOutdated = quest.Deadline < DateTime.UtcNow,
                Author = quest.Author.To(a => new StudentPartialProfileDto(a))
            };
        }
    }
}