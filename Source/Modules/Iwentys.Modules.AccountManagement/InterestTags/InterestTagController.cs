using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.InterestTags.Dto;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.AccountManagement.InterestTags.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.AccountManagement.InterestTags
{
    [Route("api/InterestTag")]
    [ApiController]
    public class InterestTagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InterestTagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<InterestTagDto>>> Get(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            GetAllTags.Response response = await _mediator.Send(new GetAllTags.Query());

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<InterestTagDto>
                .ToIndexViewModel(response.Tags, paginationFilter));
        }

        [HttpGet(nameof(GetByStudentId))]
        public async Task<ActionResult<List<InterestTagDto>>> GetByStudentId(
            [FromQuery] int studentId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            GetUserTags.Response response = await _mediator.Send(new GetUserTags.Query(studentId));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<InterestTagDto>
                .ToIndexViewModel(response.Tags, paginationFilter));
        }

        [HttpGet(nameof(Add))]
        public async Task<ActionResult> Add(int studentId, int tagId)
        {
            AddUserTag.Response response = await _mediator.Send(new AddUserTag.Query(studentId, tagId));
            return Ok();
        }

        [HttpGet(nameof(Remove))]
        public async Task<ActionResult> Remove(int studentId, int tagId)
        {
            RemoveUserTag.Response response = await _mediator.Send(new RemoveUserTag.Query(studentId, tagId));
            return Ok();
        }
    }
}