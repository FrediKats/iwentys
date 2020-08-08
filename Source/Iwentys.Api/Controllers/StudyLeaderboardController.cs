using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;
using Microsoft.AspNetCore.Http;
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
            return _studyLeaderboardService.GetAllSubjects();
        }

        [HttpGet("getSubjects/{streamId}")]
        public IEnumerable<Subject> GetSubjectsForStream(int streamId)
        {
            return _studyLeaderboardService.GetSubjectsForStream(streamId);
        }

        [HttpGet("getSubjects/{streamId}/{semester}")]
        public IEnumerable<Subject> GetSubjectsForStreamAndSemester(int streamId, StudySemester semester)
        {
            return _studyLeaderboardService.GetSubjectsForStreamAndSemester(streamId, semester);
        }

        [HttpGet("getAllGroups")]
        public IEnumerable<StudyGroup> GetAllGroups()
        {
            return _studyLeaderboardService.GetAllGroups();
        }

        [HttpGet("getGroupsFromStream/{streamId}")]
        public IEnumerable<StudyGroup> GetGroupsForStream(int streamId)
        {
            return _studyLeaderboardService.GetGroupsForStream(streamId);
        }

        [HttpGet("getGroupsFromSubject/{subjectId}")]
        public IEnumerable<StudyGroup> GetGroupsForSubject(int subjectId)
        {
            return _studyLeaderboardService.GetGroupsForSubject(subjectId);
        }

        [HttpGet("GetStudentsRating")]
        public IEnumerable<SubjectActivity> GetStudentsRating(int subjectId, int? streamId, int? groupId,
            StudySemester? semester)
        {
            return _studyLeaderboardService.GetStudentsRatings(subjectId, streamId, groupId, semester);
        }
    }
}
