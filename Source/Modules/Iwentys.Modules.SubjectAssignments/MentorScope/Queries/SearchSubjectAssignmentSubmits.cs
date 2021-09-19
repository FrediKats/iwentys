using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments.Models;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.SubjectAssignments.Dtos;
using Iwentys.Modules.SubjectAssignments.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.SubjectAssignments.MentorScope.Queries
{
    public static class SearchSubjectAssignmentSubmits
    {
        public class Query : IRequest<Response>
        {
            public Query(SubjectAssignmentSubmitSearchArguments searchArguments, AuthorizedUser authorizedUser)
            {
                SearchArguments = searchArguments;
                AuthorizedUser = authorizedUser;
            }

            public SubjectAssignmentSubmitSearchArguments SearchArguments { get; set; }
            public AuthorizedUser AuthorizedUser { get; set; }
        }

        public class Response
        {
            public Response(List<SubjectAssignmentSubmitDto> submits)
            {
                Submits = submits;
            }

            public List<SubjectAssignmentSubmitDto> Submits { get; set; }

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
                Subject subject = await _context.Subjects.GetById(request.SearchArguments.SubjectId);
                IwentysUser iwentysUser = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);
                //TODO: move to domain
                iwentysUser.EnsureIsMentor(subject);

                List<SubjectAssignmentSubmitDto> submits = await SubjectAssignmentSubmitRepository
                    .ApplySearch(_context.SubjectAssignmentSubmits, request.SearchArguments)
                    .Select(sas => new SubjectAssignmentSubmitDto(sas))
                    .ToListAsync(cancellationToken);

                return new Response(submits);
            }
        }
    }
}