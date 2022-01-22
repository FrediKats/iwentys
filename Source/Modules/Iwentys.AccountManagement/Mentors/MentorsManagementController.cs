using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.EntityManager.ApiClient;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.AccountManagement;

[Route("api/account-management/mentors")]
[ApiController]
public class MentorsManagementController : ControllerBase
{
    private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

    public MentorsManagementController(TypedIwentysEntityManagerApiClient entityManagerApiClient)
    {
        _entityManagerApiClient = entityManagerApiClient;
    }

    [HttpGet(nameof(GetAll))]
    public async Task<ActionResult<IReadOnlyList<SubjectMentorsDto>>> GetAll()
    {
        AuthorizedUser authorizedUser = this.TryAuthWithToken();
        IReadOnlyCollection<SubjectTeachersDto> result = await _entityManagerApiClient.Teachers.Client.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("by-group-subject/{groupSubjectId}")]
    public async Task<ActionResult<GroupMentorsDto>> GetByGroupSubject(int groupSubjectId)
    {
        AuthorizedUser authorizedUser = this.TryAuthWithToken();
        GroupTeachersDto result = await _entityManagerApiClient.Teachers.Client.ByGroupSubjectAsync(groupSubjectId);
        return Ok(result);
    }

    [HttpDelete(nameof(RemoveMentorFromGroup))]
    public async Task<ActionResult> RemoveMentorFromGroup([FromQuery] int groupSubjectId, [FromQuery] int mentorId)
    {
        AuthorizedUser authorizedUser = this.TryAuthWithToken();
        await _entityManagerApiClient.Teachers.Client.RemoveMentorFromGroupAsync(groupSubjectId, mentorId);
        return Ok();
    }

    [HttpPost(nameof(AddMentor))]
    public async Task<ActionResult> AddMentor([FromBody] SubjectTeacherCreateArgs args)
    {
        AuthorizedUser authorizedUser = this.TryAuthWithToken();
        await _entityManagerApiClient.Teachers.Client.AddMentorAsync(args);
        return Ok();
    }
}