using System;

namespace Iwentys.Features.Quests.Models
{
    public class CreateQuestRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime? Deadline { get; set; }
    }
}