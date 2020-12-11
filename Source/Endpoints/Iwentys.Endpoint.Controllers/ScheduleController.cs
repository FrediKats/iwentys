using System.Threading.Tasks;
using ItmoScheduleApiWrapper;
using ItmoScheduleApiWrapper.Models;
using Iwentys.Features.Study.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        public readonly ItmoApiProvider ApiProvider = new ItmoApiProvider();

        [HttpGet("today/{group}")]
        public async Task<ActionResult<ScheduleItemModel>> GetTodaySchedule(string group)
        {
            GroupScheduleModel schedule = await ApiProvider.ScheduleApi.GetGroupScheduleAsync(new GroupName(group).Name);
            return Ok(schedule.Schedule);
        }
    }
}