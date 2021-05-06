using System.Collections.Generic;
using Iwentys.Domain.AccountManagement.Dto;

namespace Iwentys.Domain.Quests.Dto
{
    public class QuestRatingRow
    {
        public int UserId { get; set; }
        public IwentysUserInfoDto User { get; set; }
        public List<int> Marks { get; set; }
    }
}