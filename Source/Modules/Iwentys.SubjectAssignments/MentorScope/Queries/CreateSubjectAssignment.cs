using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;
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


        public Handler(IwentysDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            Subject subject = await _context.Subjects.GetById(request.Arguments.SubjectId);
            IwentysUser creator = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

            var subjectAssignment = SubjectAssignment.Create(creator, subject, request.Arguments);

            _context.SubjectAssignments.Add(subjectAssignment);

            return new Response(_mapper.Map<SubjectAssignmentDto>(subjectAssignment));
        }
    }
}