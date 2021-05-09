using System;
using System.ComponentModel.DataAnnotations;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Guilds
{
    public class GuildLastLeave
    {
        [Key]
        public int UserId { get; set; }
        public virtual IwentysUser User { get; set; }

        public DateTime GuildLeftTime { get; set; }

        public void UpdateLeave()
        {
            GuildLeftTime = DateTime.UtcNow;
        }

        public bool IsLeaveRestrictExpired()
        {
            return GuildLeftTime.AddHours(24) > DateTime.UtcNow;
        }
    }
}