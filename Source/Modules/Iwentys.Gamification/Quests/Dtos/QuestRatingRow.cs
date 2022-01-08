using System.Collections.Generic;
using Iwentys.AccountManagement;

namespace Iwentys.Gamification;

public class QuestRatingRow
{
    public int UserId { get; set; }
    public IwentysUserInfoDto User { get; set; }
    public List<int> Marks { get; set; }
}