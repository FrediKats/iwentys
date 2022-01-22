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


    [HttpGet(nameof(GetGroupSubjectByTeacherId))]
    public async Task<ActionResult<List<GroupSubjectInfoDto>>> GetGroupSubjectByTeacherId(int teacherId)
    {
        GetGroupSubjectByTeacherId.Response response = await _mediator.Send(new GetGroupSubjectByTeacherId.Query(teacherId));
        return Ok(response.Groups);
    }
}