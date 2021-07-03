using Iwentys.Infrastructure.Application.Controllers.SubjectAssignments.Dtos;
using Iwentys.Infrastructure.Application.Repositories;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Iwentys.Domain.SubjectAssignments.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
    public static class GetStudentSubjectAssignmentSubmits
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; set; }
            public int SubjectId { get; set; }

            public Query(AuthorizedUser user, int subjectId)
            {
                User = user;
                SubjectId = subjectId;
            }
        }

        public class Response
        {
            public Response(List<SubjectAssignmentSubmitDto> subjectAssignments)
            {
                SubjectAssignments = subjectAssignments;
            }

            public List<SubjectAssignmentSubmitDto> SubjectAssignments { get; set; }
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
                SubjectAssignmentSubmitSearchArguments searchArguments = new SubjectAssignmentSubmitSearchArguments
                {
                    SubjectId = request.SubjectId,
                    StudentId = request.User.Id,
                };

                List<SubjectAssignmentSubmitDto> submits = await SubjectAssignmentSubmitRepository
                    .ApplySearch(_context.SubjectAssignmentSubmits, searchArguments)
                    .Select(sas => new SubjectAssignmentSubmitDto(sas))
                    .ToListAsync(cancellationToken);

                return new Response(submits);
            }
        }
    }
}
