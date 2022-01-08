using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Study;

[Route("api/[controller]")]
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

    [HttpPost(nameof(UpdateGroupSubjectTable))]
    public async Task<ActionResult<List<GroupSubjectInfoDto>>> UpdateSubjectTable(int studyGroupId, int subjectId, string tableLink)
    {
        AuthorizedUser authorizedUser = this.TryAuthWithToken();

        UpdateGroupSubjectTable.Response response = await _mediator.Send(new UpdateGroupSubjectTable.Query(authorizedUser, studyGroupId, subjectId, tableLink));

        return Ok(response.Result);
    }
}