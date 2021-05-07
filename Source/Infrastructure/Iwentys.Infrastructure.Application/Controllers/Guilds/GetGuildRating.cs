using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Guilds
{
    public class GetGuildRating
    {
        public class Query : IRequest<Response>
        {
            public Query(int skip, int take)
            {
                Skip = skip;
                Take = take;
            }

            public int Skip { get; set; }
            public int Take { get; set; }
        }

        public class Response
        {
            public Response(List<GuildProfileDto> guilds)
            {
                Guilds = guilds;
            }

            public List<GuildProfileDto> Guilds { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IGenericRepository<Guild> _guildRepository;

            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _guildRepository = _unitOfWork.GetRepository<Guild>();
            }

            protected override Response Handle(Query request)
            {
                List<GuildProfileDto> result = _guildRepository
                    .Get()
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .Select(GuildProfileDto.FromEntity)
                    .ToList()
                    .OrderByDescending(g => g.GuildRating)
                    .ToList();

                return new Response(result);
            }
        }
    }
}