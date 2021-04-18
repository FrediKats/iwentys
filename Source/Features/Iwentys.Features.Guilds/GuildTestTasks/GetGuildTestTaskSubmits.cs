using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.GuildTestTasks
{
    public static class GetGuildTestTaskSubmits
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
            public Response(List<GuildTestTaskInfoResponse> submits)
            {
                Submits = submits;
            }

            public List<GuildTestTaskInfoResponse> Submits { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IGenericRepository<GuildTestTaskSolution> _guildTestTaskSolutionRepository;

            public Handler(IUnitOfWork unitOfWork)
            {
                _guildTestTaskSolutionRepository = unitOfWork.GetRepository<GuildTestTaskSolution>();
            }

            protected override Response Handle(Query request)
            {
                List<GuildTestTaskInfoResponse> result = _guildTestTaskSolutionRepository
                    .Get()
                    .Where(t => t.GuildId == request.GuildId)
                    .Select(GuildTestTaskInfoResponse.FromEntity)
                    .ToListAsync().Result;

                return new Response(result);
            }
        }
    }
}