using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using Iwentys.Domain.Study;
using Iwentys.Features.Study.Infrastructure;
using Iwentys.Features.Study.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Gamification.Services
{
    public class StudyLeaderboardService
    {
        private readonly IGenericRepository<CourseLeaderboardRow> _courseLeaderboardRowRepository;

        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IStudyDbContext _dbContext;

        private readonly IGenericRepository<StudyGroup> _studyGroupRepository;
        private readonly IUnitOfWork _unitOfWork;


        public StudyLeaderboardService(GithubIntegrationService githubIntegrationService, IStudyDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _githubIntegrationService = githubIntegrationService;
            _dbContext = dbContext;

            _unitOfWork = unitOfWork;
            _courseLeaderboardRowRepository = _unitOfWork.GetRepository<CourseLeaderboardRow>();
            _studyGroupRepository = _unitOfWork.GetRepository<StudyGroup>();
        }

        public List<StudyLeaderboardRowDto> GetStudentsRatings(StudySearchParametersDto searchParametersDto)
        {
            if (searchParametersDto.CourseId is null && searchParametersDto.GroupId is null)
                throw new IwentysExecutionException("One of StudySearchParametersDto fields: CourseId or GroupId should be null");

            List<SubjectActivity> result = _dbContext.GetStudentActivities(searchParametersDto).ToList();

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

            List<SubjectActivity> result = _dbContext.GetStudentActivities(new StudySearchParametersDto {CourseId = courseId}).ToList();
            List<CourseLeaderboardRow> newRows = CourseLeaderboardRow.Create(courseId, result, oldRows);

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
                .AsEnumerable()
                .Select(s => new StudyLeaderboardRowDto(s, _githubIntegrationService.User.GetGithubUser(s.GithubUsername).Result?.ContributionFullInfo.Total ?? 0))
                .OrderBy(a => a.Activity)
                .Skip(skip)
                .Take(take)
                .ToList();
        }
    }
}