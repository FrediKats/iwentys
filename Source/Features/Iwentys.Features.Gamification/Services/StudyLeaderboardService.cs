using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Gamification.ViewModels;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Repositories;
using Iwentys.Features.StudentFeature.ViewModels;
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

        public Task<List<SubjectEntity>> GetSubjectsForDtoAsync(StudySearchParameters searchParameters)
        {
            return _groupSubjectRepository.GetSubjectsForDto(searchParameters).ToListAsync();
        }

        public Task<List<StudyGroupEntity>> GetStudyGroupsForDtoAsync(int? courseId)
        {
            return _groupSubjectRepository.GetStudyGroupsForDto(courseId).ToListAsync();
        }

        public List<StudyLeaderboardRow> GetStudentsRatings(StudySearchParameters searchParameters)
        {
            if (searchParameters.CourseId == null && searchParameters.GroupId == null ||
                searchParameters.CourseId != null && searchParameters.GroupId != null)
            {
                throw new IwentysException("One of StudySearchParameters fields: CourseId or GroupId should be null");
            }

            List<SubjectActivityEntity> result = _subjectActivityRepository.GetStudentActivities(searchParameters).ToList();

            return result
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRow(g))
                .OrderByDescending(a => a.Activity)
                .Skip(searchParameters.Skip)
                .Take(searchParameters.Take)
                .ToList();
        }

        public List<StudyLeaderboardRow> GetCodingRating(int? courseId, int skip, int take)
        {
            IQueryable<StudentEntity> query = _studentRepository.Read();

            query = query
                .WhereIf(courseId, q => q.Group.StudyCourseId == courseId);

            return query.AsEnumerable()
                .Select(s => new StudyLeaderboardRow(s, _githubUserDataService.FindByUsername(s.GithubUsername).Result?.ContributionFullInfo.Total ?? 0))
                .OrderBy(a => a.Activity)
                .Skip(skip)
                .Take(take)
                .ToList();
        }
    }
}
