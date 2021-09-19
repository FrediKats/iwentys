using System.Collections.Generic;
using Iwentys.Modules.AccountManagement.Dtos;

namespace Iwentys.Modules.Gamification.Quests.Dtos
{
    public class QuestRatingRow
    {
        public int UserId { get; set; }
        public IwentysUserInfoDto User { get; set; }
        public List<int> Marks { get; set; }
    }
}