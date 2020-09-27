using System.Collections.Generic;
using Iwentys.Api.Tools;
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

        [HttpGet("created")]
        public ActionResult<List<QuestInfoDto>> GetCreatedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_questService.GetCreatedByUser(user));
        }

        [HttpGet("completed")]
        public ActionResult<List<QuestInfoDto>> GetCompletedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_questService.GetCompletedByUser(user));
        }

        [HttpGet("active")]
        public ActionResult<List<QuestInfoDto>> GetActive()
        {
            return Ok(_questService.GetActive());
        }

        [HttpGet("archived")]
        public ActionResult<List<QuestInfoDto>> GetArchived()
        {
            return Ok(_questService.GetArchived());
        }

        [HttpPost]
        public ActionResult<QuestInfoDto> Create(CreateQuestDto createQuest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_questService.Create(user, createQuest));
        }

        [HttpPut("{questId}/send-response")]
        public ActionResult<QuestInfoDto> SendResponse(int questId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_questService.SendResponse(user, questId));
        }

        [HttpPut("{questId}/complete")]
        public ActionResult<QuestInfoDto> Complete([FromRoute]int questId, [FromQuery] int userId)
        {
            AuthorizedUser author = this.TryAuthWithToken();
            return Ok(_questService.Complete(author, questId, userId));
        }

        [HttpPut("{questId}/revoke")]
        public ActionResult<QuestInfoDto> Revoke([FromRoute] int questId)
        {
            AuthorizedUser author = this.TryAuthWithToken();
            return Ok(_questService.Revoke(author, questId));
        }
    }
}