using System.Threading.Tasks;
using ItmoScheduleApiWrapper;
using ItmoScheduleApiWrapper.Models;
using Iwentys.Domain.Study;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
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