using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services;
using Iwentys.Endpoints.Api.Tools;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Gamification;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoints.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly QuestService _questService;

        public QuestController(QuestService questService)
        {
            _questService = questService;
        }

        [HttpGet("created")]
        public ActionResult<List<QuestInfoResponse>> GetCreatedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_questService.GetCreatedByUser(user));
        }

        [HttpGet("completed")]
        public ActionResult<List<QuestInfoResponse>> GetCompletedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_questService.GetCompletedByUser(user));
        }

        [HttpGet("active")]
        public ActionResult<List<QuestInfoResponse>> GetActive()
        {
            return Ok(_questService.GetActive());
        }

        [HttpGet("archived")]
        public ActionResult<List<QuestInfoResponse>> GetArchived()
        {
            return Ok(_questService.GetArchived());
        }

        [HttpPost]
        public ActionResult<QuestInfoResponse> Create(CreateQuestRequest createQuest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_questService.Create(user, createQuest));
        }

        [HttpPut("{questId}/send-response")]
        public ActionResult<QuestInfoResponse> SendResponse(int questId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_questService.SendResponse(user, questId));
        }

        [HttpPut("{questId}/complete")]
        public ActionResult<QuestInfoResponse> Complete([FromRoute]int questId, [FromQuery] int userId)
        {
            AuthorizedUser author = this.TryAuthWithToken();
            return Ok(_questService.Complete(author, questId, userId));
        }

        [HttpPut("{questId}/revoke")]
        public ActionResult<QuestInfoResponse> Revoke([FromRoute] int questId)
        {
            AuthorizedUser author = this.TryAuthWithToken();
            return Ok(_questService.Revoke(author, questId));
        }
    }
}