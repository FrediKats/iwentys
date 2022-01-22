using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.EntityManager.ApiClient;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Study;

[Route("api/[controller]")]
[ApiController]
public class StudyGroupController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IwentysEntityManagerApiClient _entityManagerApiClient;

    public StudyGroupController(IMediator mediator, IwentysEntityManagerApiClient entityManagerApiClient)
    {
        _mediator = mediator;
        _entityManagerApiClient = entityManagerApiClient;
    }

    [HttpGet(nameof(GetByCourseId))]
    public async Task<ActionResult<List<StudyGroupProfileResponseDto>>> GetByCourseId([FromQuery] int? courseId)
    {
        IReadOnlyCollection<StudyGroupProfileResponseDto> result = await _entityManagerApiClient.StudyGroups.GetByCourseIdAsync(courseId);
        return Ok(result);
    }

    [HttpGet(nameof(GetByGroupName))]
    public async Task<ActionResult<StudyGroupProfileResponseDto>> GetByGroupName(string groupName)
    {
        StudyGroupProfileResponseDto result = await _entityManagerApiClient.StudyGroups.GetByGroupNameAsync(groupName);
        return Ok(result);
    }

    [HttpGet(nameof(GetByStudentId))]
    public async Task<ActionResult<StudyGroupProfileResponseDto>> GetByStudentId(int studentId)
    {
        StudyGroupProfileResponseDto result = await _entityManagerApiClient.StudyGroups.GetByStudentIdAsync(studentId);
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