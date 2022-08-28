using System.Linq;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Guilds;

public class FindStudentActiveTribute
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser user)
        {
            User = user;
        }

        public AuthorizedUser User { get; set; }
    }

    public class Response
    {
        public Response(TributeInfoResponse tribute)
        {
            Tribute = tribute;
        }

        public TributeInfoResponse Tribute { get; set; }
    }

    public class Handler : RequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        protected override Response Handle(Query request)
        {
            IwentysUser student = _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id).Result;
            TributeInfoResponse tributeInfoResponse = _context
                .Tributes
                .Where(Tribute.IsActive)
                .Where(Tribute.BelongTo(student))
                .Select(TributeInfoResponse.FromEntity)
                .SingleOrDefaultAsync().Result;

            return new Response(tributeInfoResponse);
        }
    }
}