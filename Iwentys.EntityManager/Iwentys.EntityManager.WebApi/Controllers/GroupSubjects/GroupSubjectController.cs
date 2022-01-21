using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.EntityManager.WebApi;

[Route("api/group-subject")]
[ApiController]
public class GroupSubjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupSubjectController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet(nameof(GetGroupSubjectByMentorId))]
    public async Task<ActionResult<List<GroupSubjectInfoDto>>> GetGroupSubjectByMentorId(int mentorId)
    {
        GetGroupSubjectByMentorId.Response response = await _mediator.Send(new GetGroupSubjectByMentorId.Query(mentorId));
        return Ok(response.Groups);
    }
}