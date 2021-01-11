using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.AccountManagement.Models;

namespace Iwentys.Features.Quests.Models
{
    public class QuestRatingRow
    {
        public int UserId { get; set; }
        public IwentysUserInfoDto User { get; set; }
        public List<int> Marks { get; set; }

        public int CalculateRating()
        {
            return Marks.Sum();
        }
    }
}