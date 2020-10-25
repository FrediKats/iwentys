using ItmoScheduleApiWrapper.Models;
using Iwentys.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private ScheduleService _scheduleService = new ScheduleService();

        [HttpGet("today/{group}")]
        public ActionResult<ScheduleItemModel> GetTodaySchedule(string group)
        {
            //TODO: group validation
            return Ok(_scheduleService.GetForGroup(group));
        }
    }
}