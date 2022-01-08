using Iwentys.Domain.InterestTags;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.AccountManagement
{
    public class AddUserTag
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

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            protected override Response Handle(Query request)
            {
                _context.UserInterestTags.Add(new UserInterestTag { UserId = request.UserId, InterestTagId = request.TagId });
                return new Response();
            }
        }
    }
}