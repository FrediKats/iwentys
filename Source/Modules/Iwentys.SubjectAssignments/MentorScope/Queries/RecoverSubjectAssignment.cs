using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.SubjectAssignments
{
    public class RecoverSubjectAssignment
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, int subjectAssignmentId)
            {
                SubjectAssignmentId = subjectAssignmentId;
                AuthorizedUser = authorizedUser;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
            public int SubjectAssignmentId { get; set; }
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
                SubjectAssignment subjectAssignment = await _context.SubjectAssignments.GetById(request.SubjectAssignmentId);
                IwentysUser creator = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                subjectAssignment.Recover(creator);

                _context.SubjectAssignments.Update(subjectAssignment);

                return new Response(_mapper.Map<SubjectAssignmentDto>(subjectAssignment));
            }
        }
    }
}