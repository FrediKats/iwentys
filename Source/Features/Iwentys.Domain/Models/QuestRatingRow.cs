using System.Collections.Generic;

namespace Iwentys.Domain.Models
{
    public class QuestRatingRow
    {
        public int UserId { get; set; }
        public IwentysUserInfoDto User { get; set; }
        public List<int> Marks { get; set; }
    }
}