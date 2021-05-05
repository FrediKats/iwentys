using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain.InterestTags;
using Iwentys.Domain.InterestTags.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Gamification.InterestTags
{
    public class GetUserTags
    {
        public class Query : IRequest<Response>
        {
            public Query(int userId)
            {
                UserId = userId;
            }

            public int UserId { get; set; }
        }

        public class Response
        {
            public Response(List<InterestTagDto> tags)
            {
                Tags = tags;
            }

            public List<InterestTagDto> Tags { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IGenericRepository<UserInterestTag> _userInterestTagRepository;

            public Handler(IUnitOfWork unitOfWork)
            {
                _userInterestTagRepository = unitOfWork.GetRepository<UserInterestTag>();
            }

            protected override Response Handle(Query request)
            {
                List<InterestTagDto> result = _userInterestTagRepository
                    .Get()
                    .Where(ui => ui.UserId == request.UserId)
                    .Select(ui => ui.InterestTag)
                    .Select(InterestTagDto.FromEntity)
                    .ToListAsync().Result;

                return new Response(result);
            }
        }
    }
}