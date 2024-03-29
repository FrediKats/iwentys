﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Guilds;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Guilds;

[Route("api/GuildTestTask")]
[ApiController]
public class GuildTestTaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuildTestTaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(GetByGuildId))]
    public async Task<ActionResult<List<GuildTestTaskInfoResponse>>> GetByGuildId([FromQuery] int guildId)
    {
        GetGuildTestTaskSubmits.Response response = await _mediator.Send(new GetGuildTestTaskSubmits.Query(guildId));
        return Ok(response.Submits);
    }

    [HttpPut(nameof(Accept))]
    public async Task<ActionResult<GuildTestTaskInfoResponse>> Accept([FromQuery] int guildId)
    {
        AuthorizedUser user = this.TryAuthWithToken();
        AcceptGuildTestTask.Response response = await _mediator.Send(new AcceptGuildTestTask.Query(guildId, user));
        return Ok(response.TestTaskInfo);
    }

    [HttpPut(nameof(Submit))]
    public async Task<ActionResult<GuildTestTaskInfoResponse>> Submit([FromQuery] int guildId, [FromQuery] string projectOwner, [FromQuery] string projectName)
    {
        AuthorizedUser user = this.TryAuthWithToken();
        SubmitGuildTestTask.Response response = await _mediator.Send(new SubmitGuildTestTask.Query(user, guildId, projectOwner, projectName));
        return Ok(response.TestTaskInfo);
    }

    [HttpPut(nameof(Complete))]
    public async Task<ActionResult<GuildTestTaskInfoResponse>> Complete([FromQuery] int guildId, [FromQuery] int taskSolveOwnerId)
    {
        AuthorizedUser user = this.TryAuthWithToken();
        CompleteGuildTestTask.Response response = await _mediator.Send(new CompleteGuildTestTask.Query(guildId, user, taskSolveOwnerId));
        return Ok(response.TestTaskInfo);
    }
}