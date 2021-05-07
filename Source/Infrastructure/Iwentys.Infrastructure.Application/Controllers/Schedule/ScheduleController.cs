using System.Threading.Tasks;
using Iwentys.Domain.Study;
using Kysect.ItmoScheduleSdk;
using Kysect.ItmoScheduleSdk.Models;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.Schedule
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        public readonly ItmoApiProvider ApiProvider = new ItmoApiProvider();

        [HttpGet("today/{group}")]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ScheduleItemModel>> GetToday(string group)
        {
            //TODO: try parse with valid exception
            GroupScheduleModel schedule = await ApiProvider.ScheduleApi.GetGroupScheduleAsync(new GroupName(group).Name);
            return Ok(schedule.Schedule);
        }
    }
}