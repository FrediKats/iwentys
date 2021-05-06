using Iwentys.Common.Databases;
using Iwentys.Domain.InterestTags;
using MediatR;

namespace Iwentys.Infrastructure.Application.InterestTags
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
            private readonly IGenericRepository<UserInterestTag> _userInterestTagRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _userInterestTagRepository = _unitOfWork.GetRepository<UserInterestTag>();
            }

            protected override Response Handle(Query request)
            {
                _userInterestTagRepository.Insert(new UserInterestTag { UserId = request.UserId, InterestTagId = request.TagId });
                _unitOfWork.CommitAsync().Wait();
                return new Response();
            }
        }
    }
}