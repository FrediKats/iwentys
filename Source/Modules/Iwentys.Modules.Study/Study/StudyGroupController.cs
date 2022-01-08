using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.Study.Study.Dtos;
using Iwentys.Modules.Study.Study.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Study.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudyGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet(nameof(GetByCourseId))]
        public async Task<ActionResult<List<GroupProfileResponseDto>>> GetByCourseId(
            [FromQuery] int? courseId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            GetStudyGroupByCourseId.Response response = await _mediator.Send(new GetStudyGroupByCourseId.Query(courseId));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<GroupProfileResponseDto>
                .ToIndexViewModel(response.Groups, paginationFilter));
        }

        [HttpGet(nameof(GetByGroupName))]
        public async Task<ActionResult<GroupProfileResponseDto>> GetByGroupName(string groupName)
        {
            GetStudyGroupByName.Response response = await _mediator.Send(new GetStudyGroupByName.Query(groupName));
            return Ok(response.Group);
        }

        [HttpGet(nameof(GetByStudentId))]
        public async Task<ActionResult<GroupProfileResponseDto>> GetByStudentId(int studentId)
        {
            GetStudyGroupByStudent.Response response = await _mediator.Send(new GetStudyGroupByStudent.Query(studentId));
            GroupProfileResponseDto result = response.Group;
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet(nameof(MakeGroupAdmin))]
        public async Task<ActionResult> MakeGroupAdmin(int newGroupAdminId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            MakeGroupAdmin.Response response = await _mediator.Send(new MakeGroupAdmin.Query(authorizedUser, newGroupAdminId));
            return Ok();
        }
    }
}