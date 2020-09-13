using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyLeaderboardController : ControllerBase
    {
        private readonly IStudyLeaderboardService _studyLeaderboardService;

        public StudyLeaderboardController(IStudyLeaderboardService studyLeaderboardService)
        {
            _studyLeaderboardService = studyLeaderboardService;
        }

        [HttpGet("getAllSubjects")]
        public ActionResult<IEnumerable<SubjectEntity>> GetAllSubjects()
        {
            return Ok(_studyLeaderboardService.GetSubjectsForDto(new StudySearchDto()));
        }

        [HttpGet("getSubjects/{streamId}")]
        public ActionResult<IEnumerable<SubjectEntity>> GetSubjectsForStream(int streamId)
        {
            return Ok(_studyLeaderboardService.GetSubjectsForDto(new StudySearchDto { StreamId = streamId }));
        }

        [HttpGet("getSubjects/{streamId}/{semester}")]
        public ActionResult<IEnumerable<SubjectEntity>> GetSubjectsForStreamAndSemester(int streamId, StudySemester semester)
        {
            return Ok(_studyLeaderboardService.GetSubjectsForDto(new StudySearchDto { StreamId = streamId, StudySemester = semester }));
        }

        [HttpGet("getAllGroups")]
        public ActionResult<IEnumerable<StudyGroupEntity>> GetAllGroups()
        {
            return Ok(_studyLeaderboardService.GetStudyGroupsForDto(new StudySearchDto()));
        }

        [HttpGet("getGroupsFromStream/{streamId}")]
        public ActionResult<IEnumerable<StudyGroupEntity>> GetGroupsForStream(int streamId)
        {
            return Ok(_studyLeaderboardService.GetStudyGroupsForDto(new StudySearchDto { StreamId = streamId }));
        }

        [HttpGet("getGroupsFromSubject/{subjectId}")]
        public ActionResult<IEnumerable<StudyGroupEntity>> GetGroupsForSubject(int subjectId)
        {
            return Ok(_studyLeaderboardService.GetStudyGroupsForDto(new StudySearchDto { SubjectId = subjectId }));
        }

        [HttpGet("GetStudentsRating")]
        public ActionResult<List<StudyLeaderboardRow>> GetStudentsRating(int? subjectId, int? streamId, int? groupId, StudySemester? semester)
        {
            return Ok(_studyLeaderboardService.GetStudentsRatings(new StudySearchDto
            {
                SubjectId = subjectId,
                StreamId = streamId,
                GroupId = groupId,
                StudySemester = semester

            }));
        }
    }
}
