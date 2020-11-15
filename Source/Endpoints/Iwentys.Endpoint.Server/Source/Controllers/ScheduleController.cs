using System.Collections.Generic;
using System.Threading.Tasks;
using ItmoScheduleApiWrapper.Models;
using Iwentys.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private ScheduleService _scheduleService = new ScheduleService();

        [HttpGet("today/{group}")]
        public async Task<ActionResult<ScheduleItemModel>> GetTodaySchedule(string group)
        {
            //TODO: group validation
            List<ScheduleItemModel> schedule = await _scheduleService.GetForGroup(group);
            return Ok(schedule);
        }
    }
}