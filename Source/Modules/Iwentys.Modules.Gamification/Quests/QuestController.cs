using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Achievements.Dto;
using Iwentys.Domain.Quests.Dto;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.Gamification.Quests.Dtos;
using Iwentys.Modules.Gamification.Quests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Gamification.Quests
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
        public async Task<ActionResult<List<QuestInfoDto>>> GetCreatedByUser(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetCreatedByUser.Response response = await _mediator.Send(new GetCreatedByUser.Query(user));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<QuestInfoDto>
                .ToIndexViewModel(response.QuestInfos, paginationFilter));
        }

        [HttpGet(nameof(GetCompletedByUser))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetCompletedByUser(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetCompletedByUser.Response response = await _mediator.Send(new GetCompletedByUser.Query(user));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<QuestInfoDto>
                .ToIndexViewModel(response.QuestInfos, paginationFilter));
        }

        [HttpGet(nameof(GetActive))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetActive(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetActive.Response response = await _mediator.Send(new GetActive.Query(user));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<QuestInfoDto>
                .ToIndexViewModel(response.QuestInfos, paginationFilter));
        }

        [HttpGet(nameof(GetArchived))]
        public async Task<ActionResult<List<QuestInfoDto>>> GetArchived(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetArchived.Response response = await _mediator.Send(new GetArchived.Query(user));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<QuestInfoDto>
                .ToIndexViewModel(response.QuestInfos, paginationFilter));
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
        public async Task<ActionResult<QuestInfoDto>> Complete(int questId, [FromBody] QuestCompleteArguments arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            Complete.Response response = await _mediator.Send(new Complete.Query(questId, user, arguments));
            return Ok(response.QuestInfo);
        }

        [HttpGet(nameof(Revoke))]
        public async Task<ActionResult<QuestInfoDto>> Revoke(int questId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            RevokeQuest.Response response = await _mediator.Send(new RevokeQuest.Query(questId, user));
            return Ok(response.QuestInfo);
        }

        [HttpGet(nameof(GetQuestExecutorRating))]
        public async Task<ActionResult<List<QuestRatingRow>>> GetQuestExecutorRating(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetQuestExecutorRating.Response response = await _mediator.Send(new GetQuestExecutorRating.Query(user));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<QuestRatingRow>
                .ToIndexViewModel(response.QuestRatingRows, paginationFilter));
        }
    }
}