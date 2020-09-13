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

        [HttpGet("getSubjects/{courseId}")]
        public ActionResult<IEnumerable<SubjectEntity>> GetCourseSubjects(int courseId)
        {
            return Ok(_studyLeaderboardService.GetSubjectsForDto(new StudySearchDto { CourseId = courseId }));
        }

        [HttpGet("getSubjects/{courseId}/{semester}")]
        public ActionResult<IEnumerable<SubjectEntity>> GetSubjectsForCourseAndSemester(int courseId, StudySemester semester)
        {
            return Ok(_studyLeaderboardService.GetSubjectsForDto(new StudySearchDto { CourseId = courseId, StudySemester = semester }));
        }

        [HttpGet("getAllGroups")]
        public ActionResult<IEnumerable<StudyGroupEntity>> GetAllGroups()
        {
            return Ok(_studyLeaderboardService.GetStudyGroupsForDto(new StudySearchDto()));
        }

        [HttpGet("GetCourseGroups/{courseId}")]
        public ActionResult<IEnumerable<StudyGroupEntity>> GetCourseGroups(int courseId)
        {
            return Ok(_studyLeaderboardService.GetStudyGroupsForDto(new StudySearchDto { CourseId = courseId }));
        }

        [HttpGet("GetGroupsForSubject/{subjectId}")]
        public ActionResult<IEnumerable<StudyGroupEntity>> GetGroupsForSubject(int subjectId)
        {
            return Ok(_studyLeaderboardService.GetStudyGroupsForDto(new StudySearchDto { SubjectId = subjectId }));
        }

        [HttpGet("GetStudentsRating")]
        public ActionResult<List<StudyLeaderboardRow>> GetStudentsRating(int? subjectId, int? courseId, int? groupId, StudySemester? semester)
        {
            return Ok(_studyLeaderboardService.GetStudentsRatings(new StudySearchDto
            {
                SubjectId = subjectId,
                CourseId = courseId,
                GroupId = groupId,
                StudySemester = semester

            }));
        }
    }
}
