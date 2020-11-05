using System.Collections.Generic;
using System.Threading.Tasks;
using ItmoScheduleApiWrapper;
using ItmoScheduleApiWrapper.Models;

namespace Iwentys.Core.Services
{
    public class ScheduleService
    {
        public readonly ItmoApiProvider ApiProvider = new ItmoApiProvider();

        public async Task<List<ScheduleItemModel>> GetForGroup(string group)
        {
            GroupScheduleModel schedule = await ApiProvider.ScheduleApi.GetGroupScheduleAsync(group);
            return schedule.Schedule;
        }
    }
}