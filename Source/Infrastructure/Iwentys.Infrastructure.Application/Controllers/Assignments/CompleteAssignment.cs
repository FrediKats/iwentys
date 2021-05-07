using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Assignments
{
    public class CompleteAssignment
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; }
            public int AssignmentId { get; }

            public Query(AuthorizedUser user, int assignmentId)
            {
                User = user;
                AssignmentId = assignmentId;
            }
        }

        public class Response
        {

        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<Assignment> _assignmentRepository;
            private readonly IGenericRepository<StudentAssignment> _studentAssignmentRepository;
            private readonly IGenericRepository<Student> _studentRepository;

            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _studentRepository = _unitOfWork.GetRepository<Student>();
                _assignmentRepository = _unitOfWork.GetRepository<Assignment>();
                _studentAssignmentRepository = _unitOfWork.GetRepository<StudentAssignment>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Student student = await _studentRepository.GetById(request.User.Id);
                Assignment assignment = await _assignmentRepository.GetById(request.AssignmentId);

                StudentAssignment studentAssignment = assignment.MarkCompleted(student);

                _studentAssignmentRepository.Update(studentAssignment);
                await _unitOfWork.CommitAsync();

                return new Response();
            }
        }
    }
}