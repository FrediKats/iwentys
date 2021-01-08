using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Features.Quests.Models
{
    public class QuestRatingRow
    {
        public int UserId { get; set; }
        public List<int> Marks { get; set; }

        public int CalculateRating()
        {
            return Marks.Sum();
        }
    }
}