using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.EntityManager.ApiClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IwentysEntityManagerApiClient = Iwentys.EntityManagerServiceIntegration.IwentysEntityManagerApiClient;

namespace Iwentys.Study;

[Route("api/study-courses")]
[ApiController]
public class StudyCourseController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IwentysEntityManagerApiClient _entityManagerApiClient;

    public StudyCourseController(IMediator mediator, IwentysEntityManagerApiClient entityManagerApiClient)
    {
        _mediator = mediator;
        _entityManagerApiClient = entityManagerApiClient;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudyCourseInfoDto>>> Get()
    {
        IReadOnlyCollection<StudyCourseInfoDto> result = await _entityManagerApiClient.StudyCourses.StudyCoursesAsync();
        return Ok(result);
    }
}