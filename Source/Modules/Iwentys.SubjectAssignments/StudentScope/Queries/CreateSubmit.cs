﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.SubjectAssignments;

public static class CreateSubmit
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser authorizedUser, SubjectAssignmentSubmitCreateArguments arguments)
        {
            Arguments = arguments;
            AuthorizedUser = authorizedUser;
        }

        public SubjectAssignmentSubmitCreateArguments Arguments { get; set; }
        public AuthorizedUser AuthorizedUser { get; set; }
    }

    public class Response
    {
        public Response(SubjectAssignmentSubmitDto submit)
        {
            Submit = submit;
        }

        public SubjectAssignmentSubmitDto Submit { get; set; }
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
            Student student = await _entityManagerApiClient.StudentProfiles.GetByIdAsync(request.AuthorizedUser.Id);

            GroupSubjectAssignment groupSubjectAssignment = await _context
                .GroupSubjectAssignments
                .FirstAsync(gsa => gsa.SubjectAssignmentId == request.Arguments.SubjectAssignmentId
                                   && gsa.GroupId == student.GroupId);

            SubjectAssignmentSubmit subjectAssignmentSubmit = groupSubjectAssignment.CreateSubmit(student, request.Arguments);

            _context.SubjectAssignmentSubmits.Add(subjectAssignmentSubmit);
            await _context.SaveChangesAsync();
            SubjectAssignmentSubmitDto result = await _context
                .SubjectAssignmentSubmits
                .Where(sas => sas.Id == subjectAssignmentSubmit.Id)
                .Select(sas => new SubjectAssignmentSubmitDto(sas))
                .SingleAsync();

            return new Response(result);
        }
    }
}