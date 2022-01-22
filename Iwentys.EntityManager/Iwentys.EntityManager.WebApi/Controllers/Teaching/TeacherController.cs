using Iwentys.EntityManager.PublicTypes;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.EntityManager.WebApi;

[Route("api/teachers")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeacherController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(GetAll))]
    public async Task<ActionResult<IReadOnlyList<SubjectTeachersDto>>> GetAll()
    {
        GetSubjectTeachers.Response subjectsMentors = await _mediator.Send(new GetSubjectTeachers.Query());
        return Ok(subjectsMentors.Teachers);
    }

    [HttpGet("by-group-subject/{id}")]
    public async Task<ActionResult<GroupTeachersDto>> GetByGroupSubject(int id)
    {
        var groupMentors = await _mediator.Send(new GetTeachersByGroupSubjectId.Query(id));
        return Ok(groupMentors.GroupTeachers);
    }

    [HttpDelete(nameof(RemoveMentorFromGroup))]
    public async Task<ActionResult> RemoveMentorFromGroup([FromQuery] int groupSubjectId, [FromQuery] int mentorId)
    {
        await _mediator.Send(new RemoveTeacherFromGroup.Command(groupSubjectId, mentorId));
        return Ok();
    }

    [HttpPost(nameof(AddMentor))]
    public async Task<ActionResult> AddMentor([FromBody] SubjectTeacherCreateArgs args)
    {
        await _mediator.Send(new AddTeacher.Command(args));
        return Ok();
    }

    [HttpPost(nameof(GetUserTeacherTypeForSubject))]
    public async Task<ActionResult<TeacherType>> GetUserTeacherTypeForSubject(int userId, int subjectId)
    {
        GetUserTeacherTypeForSubject.Response response = await _mediator.Send(new GetUserTeacherTypeForSubject.Query(userId, subjectId));
        return Ok(response.TeacherType);
    }

    [HttpPost(nameof(IsUserHasTeacherPermissionForSubject))]
    public async Task<ActionResult<bool>> IsUserHasTeacherPermissionForSubject(int userId, int subjectId)
    {
        GetUserTeacherTypeForSubject.Response response = await _mediator.Send(new GetUserTeacherTypeForSubject.Query(userId, subjectId));
        return Ok(response.TeacherType != TeacherType.None);
    }
}