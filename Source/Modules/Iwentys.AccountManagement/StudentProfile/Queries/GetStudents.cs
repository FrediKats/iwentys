using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Iwentys.DataAccess;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.AccountManagement;

public static class GetStudents
{
    public record Query : IRequest<Response>;
    public record Response(IReadOnlyCollection<EntityManager.ApiClient.StudentInfoDto> Students);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly IwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, IwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<EntityManager.ApiClient.StudentInfoDto> studentInfoDtos = await _entityManagerApiClient.StudentProfiles.GetAsync(cancellationToken);
            return new Response(studentInfoDtos);
        }
    }
}