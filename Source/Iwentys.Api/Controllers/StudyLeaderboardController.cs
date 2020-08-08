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

        [HttpGet]
        public IEnumerable<Subject> GetAllSubjects()
        {
            return _studyLeaderboardService.GetAllSubjects();
        }

        [HttpGet]
        public IEnumerable<Subject> GetSubjectsForStream(int streamId)
        {
            return _studyLeaderboardService.GetSubjectsForStream(streamId);
        }

        [HttpGet]
        public IEnumerable<Subject> GetSubjectsForStreamAndSemester(int streamId, StudySemester semester)
        {
            return _studyLeaderboardService.GetSubjectsForStreamAndSemester(streamId, semester);
        }

        [HttpGet]
        public IEnumerable<StudyGroup> GetAllGroups()
        {
            return _studyLeaderboardService.GetAllGroups();
        }

        [HttpGet]
        public IEnumerable<StudyGroup> GetGroupsForStream(int streamId)
        {
            return _studyLeaderboardService.GetGroupsForStream(streamId);
        }

        [HttpGet]
        public IEnumerable<StudyGroup> GetGroupsForSubject(int subjectId)
        {
            return _studyLeaderboardService.GetGroupsForSubject(subjectId);
        }

        [HttpGet]
        public IEnumerable<SubjectActivity> GetStudentsRating(int subjectId, int? streamId, int? groupId,
            StudySemester? semester)
        {
            return _studyLeaderboardService.GetStudentsRatings(subjectId, streamId, groupId, semester);
        }
    }
}
