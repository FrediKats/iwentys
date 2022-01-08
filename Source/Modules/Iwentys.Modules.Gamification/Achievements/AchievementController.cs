using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Achievements.Dto;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.Gamification.Achievements.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Gamification.Achievements
{
    [Route("api/achievements")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AchievementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetByStudentId))]
        public async Task<ActionResult<List<AchievementInfoDto>>> GetByStudentId(
            [FromQuery] int studentId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            GetByStudentId.Response response = await _mediator.Send(new GetByStudentId.Query(studentId));
            
            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<AchievementInfoDto>
                .ToIndexViewModel(response.Achievements, paginationFilter));
        }

        [HttpGet(nameof(GetByGuildId))]
        public async Task<ActionResult<List<AchievementInfoDto>>> GetByGuildId(
            [FromQuery] int guildId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            GetByGuildId.Response response = await _mediator.Send(new GetByGuildId.Query(guildId));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<AchievementInfoDto>
                .ToIndexViewModel(response.Achievements, paginationFilter));
        }
    }
}