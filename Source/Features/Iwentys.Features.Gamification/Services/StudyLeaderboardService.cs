using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Repositories;

namespace Iwentys.Features.Gamification.Services
{
    public class StudyLeaderboardService
    {
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IStudentRepository _studentRepository;
        private readonly ISubjectActivityRepository _subjectActivityRepository;

        public StudyLeaderboardService(GithubIntegrationService githubIntegrationService, IStudentRepository studentRepository, ISubjectActivityRepository subjectActivityRepository)
        {
            _githubIntegrationService = githubIntegrationService;
            _studentRepository = studentRepository;
            _subjectActivityRepository = subjectActivityRepository;
        }

        public List<StudyLeaderboardRowDto> GetStudentsRatings(StudySearchParametersDto searchParametersDto)
        {
            if (searchParametersDto.CourseId == null && searchParametersDto.GroupId == null ||
                searchParametersDto.CourseId != null && searchParametersDto.GroupId != null)
            {
                throw new IwentysException("One of StudySearchParametersDto fields: CourseId or GroupId should be null");
            }

            List<SubjectActivityEntity> result = _subjectActivityRepository.GetStudentActivities(searchParametersDto).ToList();

            return result
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRowDto(g.ToList()))
                .OrderByDescending(a => a.Activity)
                .Skip(searchParametersDto.Skip)
                .Take(searchParametersDto.Take)
                .ToList();
        }

        public List<StudyLeaderboardRowDto> GetCodingRating(int? courseId, int skip, int take)
        {
            IQueryable<StudentEntity> query = _studentRepository.Read();

            //TODO: fix
            //query = query
            //    .WhereIf(courseId, q => q.Group.StudyCourseId == courseId);

            return query.AsEnumerable()
                .Select(s => new StudyLeaderboardRowDto(s, _githubIntegrationService.FindByUsername(s.GithubUsername).Result?.ContributionFullInfo.Total ?? 0))
                .OrderBy(a => a.Activity)
                .Skip(skip)
                .Take(take)
                .ToList();
        }
    }
}
