using System.Threading.Tasks;
using ItmoScheduleApiWrapper;
using ItmoScheduleApiWrapper.Models;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        public readonly ItmoApiProvider ApiProvider = new ItmoApiProvider();

        [HttpGet("today/{group}")]
        public async Task<ActionResult<ScheduleItemModel>> GetTodaySchedule(string group)
        {
            GroupScheduleModel schedule = await ApiProvider.ScheduleApi.GetGroupScheduleAsync(group);
            //TODO: group validation
            return Ok(schedule.Schedule);
        }
    }
}