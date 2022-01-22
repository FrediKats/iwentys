using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.EntityManager.ApiClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Study;

[Route("api/[controller]")]
[ApiController]
public class GroupSubjectController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IwentysEntityManagerApiClient _entityManagerApiClient;

    public GroupSubjectController(IMediator mediator, IwentysEntityManagerApiClient entityManagerApiClient)
    {
        _mediator = mediator;
        _entityManagerApiClient = entityManagerApiClient;
    }


    [HttpGet(nameof(GetGroupSubjectByMentorId))]
    public async Task<ActionResult<List<GroupSubjectInfoDto>>> GetGroupSubjectByMentorId(int mentorId)
    {
        IReadOnlyCollection<GroupSubjectInfoDto> result = await _entityManagerApiClient.GroupSubjects.GetGroupSubjectByMentorIdAsync(mentorId);
        return Ok(result);
    }
}