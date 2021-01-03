using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Gamification.Entities;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Gamification.Services
{
    public class StudyLeaderboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<StudyGroup> _studyGroupRepository;
        private readonly IGenericRepository<CourseLeaderboardRow> _courseLeaderboardRowRepository;

        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly ISubjectActivityRepository _subjectActivityRepository;
        

        public StudyLeaderboardService(GithubIntegrationService githubIntegrationService, ISubjectActivityRepository subjectActivityRepository, IUnitOfWork unitOfWork)
        {
            _githubIntegrationService = githubIntegrationService;
            _subjectActivityRepository = subjectActivityRepository;
            
            _unitOfWork = unitOfWork;
            _courseLeaderboardRowRepository = _unitOfWork.GetRepository<CourseLeaderboardRow>();
            _studyGroupRepository = _unitOfWork.GetRepository<StudyGroup>();
        }

        public List<StudyLeaderboardRowDto> GetStudentsRatings(StudySearchParametersDto searchParametersDto)
        {
            if (searchParametersDto.CourseId is null && searchParametersDto.GroupId is null ||
                searchParametersDto.CourseId is not null && searchParametersDto.GroupId is not null)
            {
                throw new IwentysException("One of StudySearchParametersDto fields: CourseId or GroupId should be null");
            }

            List<SubjectActivity> result = _subjectActivityRepository.GetStudentActivities(searchParametersDto).ToList();

            return result
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRowDto(g.ToList()))
                .OrderByDescending(a => a.Activity)
                .Skip(searchParametersDto.Skip)
                .Take(searchParametersDto.Take)
                .ToList();
        }

        public async Task CourseRatingForceRefresh(int courseId)
        {
            List<CourseLeaderboardRow> oldRows = _courseLeaderboardRowRepository
                .Get()
                .Where(clr => clr.CourseId == courseId)
                .ToList();

            _courseLeaderboardRowRepository.Delete(oldRows);

            List<SubjectActivity> result = _subjectActivityRepository.GetStudentActivities(new StudySearchParametersDto {CourseId = courseId}).ToList();
            List<CourseLeaderboardRow> newRows = CourseLeaderboardRow.Create(courseId, result);

            await _courseLeaderboardRowRepository.InsertAsync(newRows);
            await _unitOfWork.CommitAsync();
        }

        public Task<CourseLeaderboardRow> FindStudentLeaderboardPosition(int studentId)
        {
            return _courseLeaderboardRowRepository
                .Get()
                .Where(clr => clr.StudentId == studentId)
                .FirstOrDefaultAsync();
        }

        public List<StudyLeaderboardRowDto> GetCodingRating(int? courseId, int skip, int take)
        {
            return _studyGroupRepository.Get()
                .WhereIf(courseId, q => q.StudyCourseId == courseId)
                .SelectMany(g => g.Students)
                .Select(s => s.Student)
                .AsEnumerable()
                .Select(s => new StudyLeaderboardRowDto(s, _githubIntegrationService.GetGithubUser(s.GithubUsername).Result?.ContributionFullInfo.Total ?? 0))
                .OrderBy(a => a.Activity)
                .Skip(skip)
                .Take(take)
                .ToList();
        }
    }
}
