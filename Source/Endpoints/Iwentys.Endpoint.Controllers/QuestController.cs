using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Quests.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/quests")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly QuestService _questService;

        public QuestController(QuestService questService)
        {
            _questService = questService;
        }

        [HttpGet("{questId}")]
        public async Task<ActionResult<QuestInfoDto>> Get(int questId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            QuestInfoDto quests = await _questService.Get(questId);
            return Ok(quests);
        }

        [HttpGet("created")]
        public async Task<ActionResult<List<QuestInfoDto>>> GetCreatedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<QuestInfoDto> quests = await _questService.GetCreatedByUser(user);
            return Ok(quests);
        }

        [HttpGet("completed")]
        public async Task<ActionResult<List<QuestInfoDto>>> GetCompletedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<QuestInfoDto> quests = await _questService.GetCompletedByUser(user);
            return Ok(quests);
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<QuestInfoDto>>> GetActive()
        {
            List<QuestInfoDto> quests = await _questService.GetActive();

            return Ok(quests);
        }

        [HttpGet("archived")]
        public async Task<ActionResult<List<QuestInfoDto>>> GetArchived()
        {
            List<QuestInfoDto> quests = await _questService.GetArchived();
            return Ok(quests);
        }

        [HttpPost]
        public async Task<ActionResult<QuestInfoDto>> Create(CreateQuestRequest createQuest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            QuestInfoDto quest = await _questService.Create(user, createQuest);
            return Ok(quest);
        }

        [HttpPost("{questId}/send-response")]
        public async Task<ActionResult<QuestInfoDto>> SendResponse(int questId, [FromBody] QuestResponseCreateArguments arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            QuestInfoDto quest = await _questService.SendResponse(user, questId, arguments);
            return Ok(quest);
        }

        [HttpPut("{questId}/complete")]
        public async Task<ActionResult<QuestInfoDto>> Complete([FromRoute]int questId, [FromQuery] int userId)
        {
            AuthorizedUser author = this.TryAuthWithToken();
            QuestInfoDto quest = await _questService.Complete(author, questId, userId);
            return Ok(quest);
        }

        [HttpGet("{questId}/revoke")]
        public async Task<ActionResult<QuestInfoDto>> Revoke(int questId)
        {
            AuthorizedUser author = this.TryAuthWithToken();
            QuestInfoDto quest = await _questService.Revoke(author, questId);
            return Ok(quest);
        }
    }
}