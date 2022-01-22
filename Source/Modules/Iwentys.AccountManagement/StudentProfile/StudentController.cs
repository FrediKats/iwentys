using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.AccountManagement;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IwentysEntityManagerApiClient _entityManagerApiClient;

    public StudentController(IMediator mediator, IwentysEntityManagerApiClient entityManagerApiClient)
    {
        _mediator = mediator;
        _entityManagerApiClient = entityManagerApiClient;
    }

    [HttpGet(nameof(Get))]
    public async Task<ActionResult<List<StudentInfoDto>>> Get()
    {
        IReadOnlyCollection<StudentInfoDto> result = await _entityManagerApiClient.StudentProfiles.GetAsync();
        return Ok(result);
    }

    [HttpGet(nameof(GetSelf))]
    public async Task<ActionResult<StudentInfoDto>> GetSelf()
    {
        AuthorizedUser user = this.TryAuthWithToken();
        StudentInfoDto result = await _entityManagerApiClient.StudentProfiles.GetByIdAsync(user.Id);
        return Ok(result);
    }


    [HttpGet(nameof(GetById))]
    public async Task<ActionResult<StudentInfoDto>> GetById(int id)
    {
        StudentInfoDto result = await _entityManagerApiClient.StudentProfiles.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut(nameof(UpdateProfile))]
    public async Task<ActionResult> UpdateProfile([FromBody] StudentUpdateRequestDto studentUpdateRequestDto)
    {
        AuthorizedUser authorizedUser = this.TryAuthWithToken();
        UpdateStudentProfile.Response response = await _mediator.Send(new UpdateStudentProfile.Query(authorizedUser, studentUpdateRequestDto));
        return Ok();
    }
}