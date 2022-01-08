using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Newsfeeds.Dto;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.Newsfeeds.Dtos;
using Iwentys.Modules.Newsfeeds.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Newsfeeds
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsfeedController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NewsfeedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(CreateSubjectNewsfeed))]
        public async Task<ActionResult> CreateSubjectNewsfeed(NewsfeedCreateViewModel createViewModel, int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            CreateSubjectNewsfeed.Response response = await _mediator.Send(new CreateSubjectNewsfeed.Query(createViewModel, authorizedUser, subjectId));
            return Ok();
        }

        [HttpPost(nameof(CreateGuildNewsfeed))]
        public async Task<ActionResult> CreateGuildNewsfeed(NewsfeedCreateViewModel createViewModel, int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            CreateGuildNewsfeed.Response response = await _mediator.Send(new CreateGuildNewsfeed.Query(createViewModel, authorizedUser, subjectId));
            return Ok();
        }

        [HttpGet(nameof(GetBySubjectId))]
        public async Task<ActionResult<List<NewsfeedViewModel>>> GetBySubjectId(
            [FromQuery] int subjectId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetSubjectNewsfeeds.Response response = await _mediator.Send(new GetSubjectNewsfeeds.Query(authorizedUser, subjectId));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<NewsfeedViewModel>
                .ToIndexViewModel(response.Newsfeeds, paginationFilter));
        }

        [HttpGet(nameof(GetByGuildId))]
        public async Task<ActionResult<List<NewsfeedViewModel>>> GetByGuildId(
            [FromQuery]int guildId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetGuildNewsfeeds.Response response = await _mediator.Send(new GetGuildNewsfeeds.Query(authorizedUser, guildId));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<NewsfeedViewModel>
                .ToIndexViewModel(response.Newsfeeds, paginationFilter));
        }
    }
}