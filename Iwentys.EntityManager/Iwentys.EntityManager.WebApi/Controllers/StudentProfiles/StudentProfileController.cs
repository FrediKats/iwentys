using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.EntityManager.WebApi;

[Route("api/student-profile")]
[ApiController]
public class StudentProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(Get))]
    public async Task<ActionResult<List<StudentInfoDto>>> Get()
    {
        GetStudents.Response response = await _mediator.Send(new GetStudents.Query());
        return Ok(response.Students);
    }

    [HttpGet(nameof(GetById))]
    public async Task<ActionResult<StudentInfoDto>> GetById(int id)
    {
        GetStudentById.Response response = await _mediator.Send(new GetStudentById.Query(id));
        return Ok(response.Student);
    }

    [HttpGet(nameof(GetByGroupId))]
    public async Task<ActionResult<IReadOnlyCollection<StudentInfoDto>>> GetByGroupId(int groupId)
    {
        GetStudentByGroupId.Response response = await _mediator.Send(new GetStudentByGroupId.Query(groupId));
        return Ok(response.Students);
    }

    [HttpGet(nameof(GetByCourseId))]
    public async Task<ActionResult<IReadOnlyCollection<StudentInfoDto>>> GetByCourseId(int courseId)
    {
        GetStudentByCourseId.Response response = await _mediator.Send(new GetStudentByCourseId.Query(courseId));
        return Ok(response.Students);
    }
}