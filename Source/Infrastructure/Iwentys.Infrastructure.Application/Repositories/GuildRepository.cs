using System;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.DataAccess;

namespace Iwentys.Infrastructure.Application.Repositories
{
    public class GuildRepository
    {
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
                guildLastLeaveRepository.Insert(lastLeave);
            }

            return lastLeave;
        }
    }
}