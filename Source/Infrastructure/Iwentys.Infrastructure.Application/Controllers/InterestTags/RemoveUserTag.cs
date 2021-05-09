using Iwentys.Domain.InterestTags;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.InterestTags
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

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IGenericRepository<UserInterestTag> _userInterestTagRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _userInterestTagRepository = _unitOfWork.GetRepository<UserInterestTag>();
            }

            protected override Response Handle(Query request)
            {
                _userInterestTagRepository.Delete(new UserInterestTag { UserId = request.UserId, InterestTagId = request.TagId });
                _unitOfWork.CommitAsync().Wait();
                return new Response();
            }
        }
    }
}