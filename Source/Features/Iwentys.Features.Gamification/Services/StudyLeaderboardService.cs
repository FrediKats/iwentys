using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Gamification.Services
{
    public class StudyLeaderboardService
    {
        private readonly GithubUserDataService _githubUserDataService;
        private readonly IStudentRepository _studentRepository;
        private readonly ISubjectActivityRepository _subjectActivityRepository;
        private readonly IGroupSubjectRepository _groupSubjectRepository;

        public StudyLeaderboardService(GithubUserDataService githubUserDataService, IStudentRepository studentRepository, ISubjectActivityRepository subjectActivityRepository, IGroupSubjectRepository groupSubjectRepository)
        {
            _githubUserDataService = githubUserDataService;
            _studentRepository = studentRepository;
            _subjectActivityRepository = subjectActivityRepository;
            _groupSubjectRepository = groupSubjectRepository;
        }

        public Task<List<SubjectEntity>> GetSubjectsForDtoAsync(StudySearchParametersDto searchParametersDto)
        {
            return _groupSubjectRepository.GetSubjectsForDto(searchParametersDto).ToListAsync();
        }

        public Task<List<StudyGroupEntity>> GetStudyGroupsForDtoAsync(int? courseId)
        {
            return _groupSubjectRepository.GetStudyGroupsForDto(courseId).ToListAsync();
        }

        public List<StudyLeaderboardRow> GetStudentsRatings(StudySearchParametersDto searchParametersDto)
        {
            if (searchParametersDto.CourseId == null && searchParametersDto.GroupId == null ||
                searchParametersDto.CourseId != null && searchParametersDto.GroupId != null)
            {
                throw new IwentysException("One of StudySearchParametersDto fields: CourseId or GroupId should be null");
            }

            List<SubjectActivityEntity> result = _subjectActivityRepository.GetStudentActivities(searchParametersDto).ToList();

            return result
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRow(g))
                .OrderByDescending(a => a.Activity)
                .Skip(searchParametersDto.Skip)
                .Take(searchParametersDto.Take)
                .ToList();
        }

        public List<StudyLeaderboardRow> GetCodingRating(int? courseId, int skip, int take)
        {
            IQueryable<StudentEntity> query = _studentRepository.Read();

            //TODO: fix
            //query = query
            //    .WhereIf(courseId, q => q.Group.StudyCourseId == courseId);

            return query.AsEnumerable()
                .Select(s => new StudyLeaderboardRow(s, _githubUserDataService.FindByUsername(s.GithubUsername).Result?.ContributionFullInfo.Total ?? 0))
                .OrderBy(a => a.Activity)
                .Skip(skip)
                .Take(take)
                .ToList();
        }
    }
}
