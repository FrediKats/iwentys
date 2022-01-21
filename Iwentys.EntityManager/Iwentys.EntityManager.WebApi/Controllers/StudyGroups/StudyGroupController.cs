using Iwentys.EntityManager.WebApiDtos;
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
    public async Task<ActionResult<List<StudyGroupProfileResponseDto>>> GetByCourseId([FromQuery] int? courseId)
    {
        GetStudyGroupByCourseId.Response response = await _mediator.Send(new GetStudyGroupByCourseId.Query(courseId));
        return Ok(response.Groups);
    }

    [HttpGet(nameof(GetByGroupName))]
    public async Task<ActionResult<StudyGroupProfileResponseDto>> GetByGroupName(string groupName)
    {
        GetStudyGroupByName.Response response = await _mediator.Send(new GetStudyGroupByName.Query(groupName));
        return Ok(response.StudyGroup);
    }

    [HttpGet(nameof(GetByStudentId))]
    public async Task<ActionResult<StudyGroupProfileResponseDto>> GetByStudentId(int studentId)
    {
        GetStudyGroupByStudent.Response response = await _mediator.Send(new GetStudyGroupByStudent.Query(studentId));
        StudyGroupProfileResponseDto result = response.StudyGroup;
        if (result is null)
            return NotFound();

        return Ok(result);
    }
}