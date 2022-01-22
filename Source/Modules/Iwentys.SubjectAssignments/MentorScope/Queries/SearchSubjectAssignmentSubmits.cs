using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.SubjectAssignments;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            bool hasPermission = await _entityManagerApiClient.Teachers.Client.IsUserHasTeacherPermissionForSubjectAsync(request.AuthorizedUser.Id, request.SearchArguments.SubjectId);
            if (!hasPermission)
                throw InnerLogicException.StudyExceptions.UserHasNotTeacherPermission(request.AuthorizedUser.Id);

            List<SubjectAssignmentSubmitDto> submits = await SubjectAssignmentSubmitRepository
                .ApplySearch(_context.SubjectAssignmentSubmits, request.SearchArguments)
                .Select(sas => new SubjectAssignmentSubmitDto(sas))
                .ToListAsync(cancellationToken);

            return new Response(submits);
        }
    }
}