using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study;
using Iwentys.Features.Study.Infrastructure;
using Iwentys.Features.Study.Repositories;
using MediatR;

namespace Iwentys.Features.Gamification.StudyLeaderboard
{
    public class CourseRatingForceRefresh
    {
        public class Query : IRequest<Response>
        {
            public Query(int courseId)
            {
                CourseId = courseId;
            }

            public int CourseId { get; set; }
        }

        public class Response
        {
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<CourseLeaderboardRow> _courseLeaderboardRowRepository;

            private readonly IStudyDbContext _dbContext;

            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork, IGenericRepository<CourseLeaderboardRow> courseLeaderboardRowRepository, IStudyDbContext dbContext)
            {
                _unitOfWork = unitOfWork;
                _courseLeaderboardRowRepository = courseLeaderboardRowRepository;
                _dbContext = dbContext;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                List<CourseLeaderboardRow> oldRows = _courseLeaderboardRowRepository
                    .Get()
                    .Where(clr => clr.CourseId == request.CourseId)
                    .ToList();

                _courseLeaderboardRowRepository.Delete(oldRows);

                List<SubjectActivity> result = _dbContext.GetStudentActivities(new StudySearchParametersDto { CourseId = request.CourseId }).ToList();
                List<CourseLeaderboardRow> newRows = CourseLeaderboardRow.Create(request.CourseId, result, oldRows);

                await _courseLeaderboardRowRepository.InsertAsync(newRows);
                await _unitOfWork.CommitAsync();

                return new Response();
            }
        }
    }
}