using System;

namespace Iwentys.Features.Quests.Models
{
    public record CreateQuestRequest
    {
        public CreateQuestRequest(string title, string description, int price, DateTime? deadline)
        {
            Title = title;
            Description = description;
            Price = price;
            Deadline = deadline;
        }

        public CreateQuestRequest()
        {
        }
        
        public string Title { get; init; }
        public string Description { get; init; }
        public int Price { get; init; }
        public DateTime? Deadline { get; init; }
    }
}