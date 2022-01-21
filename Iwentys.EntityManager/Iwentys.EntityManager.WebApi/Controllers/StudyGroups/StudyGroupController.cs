using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.EntityManager.WebApi;

[Route("api/study-group")]
[ApiController]
public class StudyGroupController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudyGroupController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet(nameof(GetByCourseId))]
    public async Task<ActionResult<List<GroupProfileResponseDto>>> GetByCourseId([FromQuery] int? courseId)
    {
        GetStudyGroupByCourseId.Response response = await _mediator.Send(new GetStudyGroupByCourseId.Query(courseId));
        return Ok(response.Groups);
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
}