using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Quests.Services;
using Iwentys.Features.Students.Domain;
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
        public async Task<ActionResult<QuestInfoResponse>> Get(int questId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            QuestInfoResponse quests = await _questService.Get(questId);
            return Ok(quests);
        }

        [HttpGet("created")]
        public async Task<ActionResult<List<QuestInfoResponse>>> GetCreatedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<QuestInfoResponse> quests = await _questService.GetCreatedByUserAsync(user);
            return Ok(quests);
        }

        [HttpGet("completed")]
        public async Task<ActionResult<List<QuestInfoResponse>>> GetCompletedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<QuestInfoResponse> quests = await _questService.GetCompletedByUserAsync(user);
            return Ok(quests);
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<QuestInfoResponse>>> GetActive()
        {
            List<QuestInfoResponse> quests = await _questService.GetActiveAsync();

            return Ok(quests);
        }

        [HttpGet("archived")]
        public async Task<ActionResult<List<QuestInfoResponse>>> GetArchived()
        {
            List<QuestInfoResponse> quests = await _questService.GetArchivedAsync();
            return Ok(quests);
        }

        [HttpPost]
        public async Task<ActionResult<QuestInfoResponse>> Create(CreateQuestRequest createQuest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            QuestInfoResponse quest = await _questService.CreateAsync(user, createQuest);
            return Ok(quest);
        }

        //TODO: send other info about response
        [HttpGet("{questId}/send-response")]
        public async Task<ActionResult<QuestInfoResponse>> SendResponse(int questId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            QuestInfoResponse quest = await _questService.SendResponseAsync(user, questId);
            return Ok(quest);
        }

        [HttpPut("{questId}/complete")]
        public async Task<ActionResult<QuestInfoResponse>> Complete([FromRoute]int questId, [FromQuery] int userId)
        {
            AuthorizedUser author = this.TryAuthWithToken();
            QuestInfoResponse quest = await _questService.CompleteAsync(author, questId, userId);
            return Ok(quest);
        }

        [HttpPut("{questId}/revoke")]
        public async Task<ActionResult<QuestInfoResponse>> Revoke([FromRoute] int questId)
        {
            AuthorizedUser author = this.TryAuthWithToken();
            QuestInfoResponse quest = await _questService.RevokeAsync(author, questId);
            return Ok(quest);
        }
    }
}