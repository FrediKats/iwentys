using System;

namespace Iwentys.Features.Quests.Models
{
    public record CreateQuestRequest(
        string Title,
        string Description,
        int Price,
        DateTime? Deadline)
    {
    }
}