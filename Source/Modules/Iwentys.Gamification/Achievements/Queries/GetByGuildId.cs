using System.Collections.Generic;
using System.Linq;
using Iwentys.DataAccess;
using Iwentys.Domain.Achievements;
using MediatR;

namespace Iwentys.Gamification;

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
        private readonly IwentysDbContext _context;

        public Handler(IwentysDbContext context)
        {
            _context = context;
        }

        protected override Response Handle(Query request)
        {
            List<AchievementInfoDto> result = _context
                .GuildAchievements
                .Where(a => a.GuildId == request.GuildId)
                .Select(AchievementInfoDto.FromGuildAchievement)
                .ToList();

            return new Response(result);
        }
    }
}