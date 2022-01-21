using Iwentys.EntityManager.PublicTypes;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.EntityManager.WebApi;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(SearchSubjects))]
    public async Task<ActionResult<List<SubjectProfileDto>>> SearchSubjects(int? courseId, StudySemester? semester)
    {
        var studySearchParameters = new SubjectSearchParametersDto(null, null, courseId, semester, 0, 20);
        SearchSubjects.Response response = await _mediator.Send(new SearchSubjects.Query(studySearchParameters));

        return Ok(response.Subjects);
    }

    [HttpGet(nameof(GetSubjectById))]
    public async Task<ActionResult<SubjectProfileDto>> GetSubjectById(int subjectId)
    {
        GetSubjectById.Response response = await _mediator.Send(new GetSubjectById.Query(subjectId));
        return Ok(response.Subject);
    }

    [HttpGet(nameof(GetSubjectsByGroupId))]
    public async Task<ActionResult<List<SubjectProfileDto>>> GetSubjectsByGroupId(int groupId)
    {
        GetSubjectsByGroupId.Response response = await _mediator.Send(new GetSubjectsByGroupId.Query(groupId));
        return Ok(response.Subjects);
    }
}