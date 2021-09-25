using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.Application.Repositories;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Study.Study.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.Study.Study.Queries
{
    public class SearchSubjects
    {
        public class Query : IRequest<Response>
        {
            public StudySearchParametersDto SearchParametersDto { get; set; }

            public Query(StudySearchParametersDto searchParametersDto)
            {
                SearchParametersDto = searchParametersDto;
            }
        }

        public class Response
        {
            public Response(List<SubjectProfileDto> subjects)
            {
                Subjects = subjects;
            }

            public List<SubjectProfileDto> Subjects { get; set; }
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
                List<SubjectProfileDto> result = await _context
                    .GroupSubjects
                    .SearchSubjects(request.SearchParametersDto)
                    .Select(entity => new SubjectProfileDto(entity))
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}