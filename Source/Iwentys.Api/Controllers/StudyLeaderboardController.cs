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
        public IEnumerable<Subject> GetAllSubjects()
        {
            return _studyLeaderboardService.GetSubjectsForDto(new StudySearchDto());
        }

        [HttpGet("getSubjects/{streamId}")]
        public IEnumerable<Subject> GetSubjectsForStream(int streamId)
        {
            return _studyLeaderboardService.GetSubjectsForDto(new StudySearchDto {StreamId = streamId});
        }

        [HttpGet("getSubjects/{streamId}/{semester}")]
        public IEnumerable<Subject> GetSubjectsForStreamAndSemester(int streamId, StudySemester semester)
        {
            return _studyLeaderboardService.GetSubjectsForDto(new StudySearchDto {StreamId = streamId, StudySemester = semester});
        }

        [HttpGet("getAllGroups")]
        public IEnumerable<StudyGroup> GetAllGroups()
        {
            return _studyLeaderboardService.GetStudyGroupsForDto(new StudySearchDto());
        }

        [HttpGet("getGroupsFromStream/{streamId}")]
        public IEnumerable<StudyGroup> GetGroupsForStream(int streamId)
        {
            return _studyLeaderboardService.GetStudyGroupsForDto(new StudySearchDto{StreamId = streamId});
        }

        [HttpGet("getGroupsFromSubject/{subjectId}")]
        public IEnumerable<StudyGroup> GetGroupsForSubject(int subjectId)
        {
            return _studyLeaderboardService.GetStudyGroupsForDto(new StudySearchDto{SubjectId = subjectId});
        }

        [HttpGet("GetStudentsRating")]
        public IEnumerable<SubjectActivity> GetStudentsRating(int subjectId, int? streamId, int? groupId,
            StudySemester? semester)
        {
            return _studyLeaderboardService.GetStudentsRatings(new StudySearchDto 
                { SubjectId = subjectId, StreamId = streamId, GroupId = groupId, StudySemester = semester });
        }
    }
}
