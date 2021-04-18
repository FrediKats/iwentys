using System.Collections.Generic;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Extended.Models
{
    public class QuestRatingRow
    {
        public int UserId { get; set; }
        public IwentysUserInfoDto User { get; set; }
        public List<int> Marks { get; set; }
    }
}