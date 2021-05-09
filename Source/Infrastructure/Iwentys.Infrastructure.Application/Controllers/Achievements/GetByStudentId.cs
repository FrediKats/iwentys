using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Achievements.Dto;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Achievements
{
    public class GetByStudentId
    {
        public class Query : IRequest<Response>
        {
            public Query(int studentId)
            {
                StudentId = studentId;
            }

            public int StudentId { get; set; }
        }

        public class Response
        {
            public Response(List<AchievementInfoDto> achievements)
            {
                Achievements = achievements;
            }

            public List<AchievementInfoDto> Achievements { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IGenericRepository<Achievement> _achievementRepository;
            private readonly IGenericRepository<GuildAchievement> _guildAchievementRepository;
            private readonly IGenericRepository<StudentAchievement> _studentAchievementRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _achievementRepository = _unitOfWork.GetRepository<Achievement>();
                _guildAchievementRepository = _unitOfWork.GetRepository<GuildAchievement>();
                _studentAchievementRepository = _unitOfWork.GetRepository<StudentAchievement>();
            }

            protected override Response Handle(Query request)
            {
                List<AchievementInfoDto> result = _studentAchievementRepository
                    .Get()
                    .Where(a => a.StudentId == request.StudentId)
                    .Select(AchievementInfoDto.FromStudentsAchievement)
                    .ToList();

                return new Response(result);
            }
        }
    }
}
