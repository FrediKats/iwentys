using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Quests.Dto;
using Iwentys.Infrastructure.Application.Infrastructure;
using Iwentys.Infrastructure.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Quests
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

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<QuestInfoDto>> GetById(int questId)
        {
            QuestInfoDto quests = await _questService.Get(questId);
            return Ok(quests);
        }

        [HttpGet(nameof(GetCreatedByUser))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetCreatedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<QuestInfoDto> quests = await _questService.GetCreatedByUser(user);
            return Ok(quests);
        }

        [HttpGet(nameof(GetCompletedByUser))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetCompletedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<QuestInfoDto> quests = await _questService.GetCompletedByUser(user);
            return Ok(quests);
        }

        [HttpGet(nameof(GetActive))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetActive()
        {
            List<QuestInfoDto> quests = await _questService.GetActive();

            return Ok(quests);
        }

        [HttpGet(nameof(GetArchived))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetArchived()
        {
            List<QuestInfoDto> quests = await _questService.GetArchived();
            return Ok(quests);
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<QuestInfoDto>> Create(CreateQuestRequest createQuest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            QuestInfoDto quest = await _questService.Create(user, createQuest);
            return Ok(quest);
        }

        [HttpPost(nameof(SendResponse))]
        public async Task<ActionResult<QuestInfoDto>> SendResponse(int questId, [FromBody] QuestResponseCreateArguments arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            QuestInfoDto quest = await _questService.SendResponse(user, questId, arguments);
            return Ok(quest);
        }

        [HttpPut(nameof(Complete))]
        public async Task<ActionResult<QuestInfoDto>> Complete([FromRoute]int questId, [FromBody] QuestCompleteArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            QuestInfoDto quest = await _questService.Complete(authorizedUser, questId, arguments);
            return Ok(quest);
        }

        [HttpGet(nameof(Revoke))]
        public async Task<ActionResult<QuestInfoDto>> Revoke(int questId)
        {
            AuthorizedUser author = this.TryAuthWithToken();
            QuestInfoDto quest = await _questService.Revoke(author, questId);
            return Ok(quest);
        }

        [HttpGet(nameof(GetQuestExecutorRating))]
        public async Task<ActionResult<List<QuestRatingRow>>> GetQuestExecutorRating()
        {
            List<QuestRatingRow> result = await _questService.GetQuestExecutorRating();
            return result;
        }
    }
}