using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.EntityManager.WebApi;

[Route("api/study-courses")]
[ApiController]
public class StudyCourseController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudyCourseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudyCourseInfoDto>>> Get()
    {
        GetStudyCourses.Response response = await _mediator.Send(new GetStudyCourses.Query());
        return Ok(response.Courses);
    }
}