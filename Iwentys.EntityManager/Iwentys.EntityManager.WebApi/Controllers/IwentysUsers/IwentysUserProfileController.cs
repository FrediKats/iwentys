using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.EntityManager.WebApi;

[Route("api/iwentys-user-profile")]
[ApiController]
public class IwentysUserProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public IwentysUserProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(Get))]
    public async Task<ActionResult<List<IwentysUserInfoDto>>> Get()
    {
        GetIwentysUsers.Response response = await _mediator.Send(new GetIwentysUsers.Query());
        return Ok(response.Users);
    }

    [HttpGet(nameof(GetById))]
    public async Task<ActionResult<IwentysUserInfoDto>> GetById(int id)
    {
        GetIwentysUserById.Response response = await _mediator.Send(new GetIwentysUserById.Query(id));
        return Ok(response.User);
    }
}