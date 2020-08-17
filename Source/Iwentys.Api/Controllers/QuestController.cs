using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Gamification;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly IQuestService _questService;

        public QuestController(IQuestService questService)
        {
            _questService = questService;
        }

        [HttpGet("GetCreatedByUser")]
        public ActionResult<List<QuestInfoDto>> GetCreatedByUser()
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_questService.GetCreatedByUser(user));
        }

        [HttpGet("GetCompletedByUser")]
        public ActionResult<List<QuestInfoDto>> GetCompletedByUser()
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_questService.GetCompletedByUser(user));
        }

        [HttpGet("GetActive")]
        public ActionResult<List<QuestInfoDto>> GetActive()
        {
            return Ok(_questService.GetActive());
        }

        [HttpGet("GetArchive")]
        public ActionResult<List<QuestInfoDto>> GetArchive()
        {
            return Ok(_questService.GetArchive());
        }

        [HttpPost("{questId}/SendResponse")]
        public ActionResult<QuestInfoDto> SendResponse(int questId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_questService.SendResponse(user, questId));
        }

        [HttpPost("{questId}/SetCompleted/{")]
        public ActionResult<QuestInfoDto> SetCompleted([FromRoute]int questId, [FromQuery] int userId)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();
            return Ok(_questService.SetCompleted(user, questId, userId));
        }
    }
}