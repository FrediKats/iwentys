﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.SubjectAssignments;

public static class CreateSubjectAssignment
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser authorizedUser, SubjectAssignmentCreateArguments arguments)
        {
            Arguments = arguments;
            AuthorizedUser = authorizedUser;
        }

        public AuthorizedUser AuthorizedUser { get; set; }
        public SubjectAssignmentCreateArguments Arguments { get; set; }
    }

    public class Response
    {
        public Response(SubjectAssignmentDto subjectAssignment)
        {
            SubjectAssignment = subjectAssignment;
        }

        public SubjectAssignmentDto SubjectAssignment { get; set; }
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly IMapper _mapper;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;


        public Handler(IwentysDbContext context, IMapper mapper, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _mapper = mapper;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser creator = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);

            bool hasPermission = await _entityManagerApiClient.Teachers.HasTeacherPermissionAsync(request.AuthorizedUser.Id, request.Arguments.SubjectId);
            if (!hasPermission)
                throw InnerLogicException.StudyExceptions.UserHasNotTeacherPermission(request.AuthorizedUser.Id);

            var subjectAssignment = SubjectAssignment.Create(creator, request.Arguments.SubjectId, request.Arguments);

            _context.SubjectAssignments.Add(subjectAssignment);

            return new Response(_mapper.Map<SubjectAssignmentDto>(subjectAssignment));
        }
    }
}