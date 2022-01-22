using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.EntityManager.ApiClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudySemester = Iwentys.EntityManager.ApiClient.StudySemester;

namespace Iwentys.Study;

[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IwentysEntityManagerApiClient _entityManagerApiClient;

    public SubjectController(IMediator mediator, IwentysEntityManagerApiClient entityManagerApiClient)
    {
        _mediator = mediator;
        _entityManagerApiClient = entityManagerApiClient;
    }

    [HttpGet(nameof(SearchSubjects))]
    public async Task<ActionResult<List<SubjectProfileDto>>> SearchSubjects(int? courseId, StudySemester semester)
    {
        IReadOnlyCollection<SubjectProfileDto> result = await _entityManagerApiClient.Subjects.SearchSubjectsAsync(courseId, semester);
        return Ok(result);
    }

    [HttpGet(nameof(GetSubjectById))]
    public async Task<ActionResult<SubjectProfileDto>> GetSubjectById(int subjectId)
    {
        SubjectProfileDto subjectProfileDto = await _entityManagerApiClient.Subjects.GetSubjectByIdAsync(subjectId);
        return Ok(subjectProfileDto);
    }

    [HttpGet(nameof(GetSubjectsByGroupId))]
    public async Task<ActionResult<List<SubjectProfileDto>>> GetSubjectsByGroupId(int groupId)
    {
        IReadOnlyCollection<SubjectProfileDto> subjectProfileDtos = await _entityManagerApiClient.Subjects.GetSubjectsByGroupIdAsync(groupId);
        return Ok(subjectProfileDtos);
    }
}