using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.SubjectAssignments
{
    public static class UpdateSubjectAssignment
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, SubjectAssignmentUpdateArguments arguments)
            {
                Arguments = arguments;
                AuthorizedUser = authorizedUser;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
            public SubjectAssignmentUpdateArguments Arguments { get; set; }
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


            public Handler(IwentysDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                SubjectAssignment subjectAssignment = await _context.SubjectAssignments.GetById(request.Arguments.SubjectAssignmentId);
                IwentysUser creator = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                subjectAssignment.Update(creator, request.Arguments);

                _context.SubjectAssignments.Update(subjectAssignment);

                return new Response(_mapper.Map<SubjectAssignmentDto>(subjectAssignment));
            }
        }
    }
}