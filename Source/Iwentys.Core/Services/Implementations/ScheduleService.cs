using System.Collections.Generic;
using ItmoScheduleApiWrapper;
using ItmoScheduleApiWrapper.Models;

namespace Iwentys.Core.Services.Implementations
{
    public class ScheduleService
    {
        public readonly ItmoApiProvider ApiProvider = new ItmoApiProvider();

        public List<ScheduleItemModel> GetForGroup(string group)
        {
            return ApiProvider.ScheduleApi.GetGroupScheduleAsync(group).Result.Schedule;
        }
    }
}