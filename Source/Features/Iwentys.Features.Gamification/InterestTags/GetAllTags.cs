using System.Collections.Generic;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.InterestTags;
using Iwentys.Domain.InterestTags.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Gamification.InterestTags
{
    public class GetAllTags
    {
        public class Query : IRequest<Response>
        {
            public Query()
            {
            }
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
            private readonly IGenericRepository<InterestTag> _interestTagRepository;

            public Handler(IUnitOfWork unitOfWork)
            {
                _interestTagRepository = unitOfWork.GetRepository<InterestTag>();
            }

            protected override Response Handle(Query request)
            {
                List<InterestTag> interestTagEntities = _interestTagRepository.Get().ToListAsync().Result;
                return new Response(interestTagEntities.SelectToList(t => new InterestTagDto(t)));
            }
        }
    }
}