using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Achievements.Dto;
using MediatR;

namespace Iwentys.Infrastructure.Application.Achievements
{
    public class GetByGuildId
    {
        public class Query : IRequest<Response>
        {
            public Query(int guildId)
            {
                GuildId = guildId;
            }

            public int GuildId { get; set; }
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
                List<AchievementInfoDto> result = _guildAchievementRepository
                    .Get()
                    .Where(a => a.GuildId == request.GuildId)
                    .Select(AchievementInfoDto.FromGuildAchievement)
                    .ToList();

                return new Response(result);
            }
        }
    }
}