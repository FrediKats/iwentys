using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.InterestTags;
using MediatR;

namespace Iwentys.AccountManagement
{
    public class RemoveUserTag
    {
        public class Query : IRequest<Response>
        {
            public Query(int userId, int tagId)
            {
                UserId = userId;
                TagId = tagId;
            }
            public int UserId { get; set; }
            public int TagId { get; set; }
        }

        public class Response
        {
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                _context.UserInterestTags.Remove(new UserInterestTag { UserId = request.UserId, InterestTagId = request.TagId });
                await _context.SaveChangesAsync();
                return new Response();
            }
        }
    }
}