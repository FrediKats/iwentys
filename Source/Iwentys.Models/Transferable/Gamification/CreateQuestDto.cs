using System;

namespace Iwentys.Models.Transferable.Gamification
{
    public class CreateQuestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime? Deadline { get; set; }
    }
}