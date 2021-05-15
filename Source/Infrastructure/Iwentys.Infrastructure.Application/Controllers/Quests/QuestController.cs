using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Quests.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.Quests
{
    [Route("api/quests")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<QuestInfoDto>> GetById(int questId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetQuestById.Response response = await _mediator.Send(new GetQuestById.Query(questId, user));
            return Ok(response.QuestInfo);
        }

        [HttpGet(nameof(GetCreatedByUser))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetCreatedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetCreatedByUser.Response response = await _mediator.Send(new GetCreatedByUser.Query(user));
            return Ok(response.QuestInfos);
        }

        [HttpGet(nameof(GetCompletedByUser))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetCompletedByUser()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetCompletedByUser.Response response = await _mediator.Send(new GetCompletedByUser.Query(user));
            return Ok(response.QuestInfos);
        }

        [HttpGet(nameof(GetActive))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetActive()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetActive.Response response = await _mediator.Send(new GetActive.Query(user));
            return Ok(response.QuestInfos);
        }

        [HttpGet(nameof(GetArchived))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetArchived()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetArchived.Response response = await _mediator.Send(new GetArchived.Query(user));
            return Ok(response.QuestInfos);
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<QuestInfoDto>> Create(CreateQuestRequest createQuest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            Create.Response response = await _mediator.Send(new Create.Query(user, createQuest));
            return Ok(response.QuestInfo);
        }

        [HttpPost(nameof(SendResponse))]
        public async Task<ActionResult<QuestInfoDto>> SendResponse(int questId, [FromBody] QuestResponseCreateArguments arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            SendResponse.Response response = await _mediator.Send(new SendResponse.Query(questId, user, arguments));
            return Ok(response.QuestInfo);
        }

        [HttpPut(nameof(Complete))]
        public async Task<ActionResult<QuestInfoDto>> Complete([FromRoute]int questId, [FromBody] QuestCompleteArguments arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            Complete.Response response = await _mediator.Send(new Complete.Query(questId, user, arguments));
            return Ok(response.QuestInfo);
        }

        [HttpGet(nameof(Revoke))]
        public async Task<ActionResult<QuestInfoDto>> Revoke(int questId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            Revoke.Response response = await _mediator.Send(new Revoke.Query(questId, user));
            return Ok(response.QuestInfo);
        }

        [HttpGet(nameof(GetQuestExecutorRating))]
        public async Task<ActionResult<List<QuestRatingRow>>> GetQuestExecutorRating()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetQuestExecutorRating.Response response = await _mediator.Send(new GetQuestExecutorRating.Query(user));
            return Ok(response.QuestRatingRows);
        }
    }
}