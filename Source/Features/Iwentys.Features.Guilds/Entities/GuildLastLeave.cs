using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildLastLeave
    {
        [Key]
        public int UserId { get; set; }
        public virtual IwentysUser User { get; set; }

        public DateTime GuildLeftTime { get; set; }

        public static async Task<GuildLastLeave> Get(IwentysUser user, IGenericRepository<GuildLastLeave> guildLastLeaveRepository)
        {
            GuildLastLeave lastLeave = await guildLastLeaveRepository.FindByIdAsync(user.Id);
            if (lastLeave is null)
            {
                lastLeave = new GuildLastLeave
                {
                    UserId = user.Id,
                    GuildLeftTime = DateTime.UnixEpoch
                };
                await guildLastLeaveRepository.InsertAsync(lastLeave);
            }

            return lastLeave;
        }

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