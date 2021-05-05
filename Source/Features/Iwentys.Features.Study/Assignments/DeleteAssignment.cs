using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Assignments
{
    public class DeleteAssignment
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

                if (student.Id != assignment.AuthorId)
                    throw InnerLogicException.AssignmentExceptions.IsNotAssignmentCreator(assignment.Id, student.Id);

                //FYI: it's coz for dropped cascade. Need to rework after adding cascade deleting
                List<StudentAssignment> studentAssignments = await _studentAssignmentRepository
                    .Get()
                    .Where(sa => sa.AssignmentId == request.AssignmentId)
                    .ToListAsync();

                _studentAssignmentRepository.Delete(studentAssignments);
                _assignmentRepository.Delete(assignment);

                await _unitOfWork.CommitAsync();

                return new Response();
            }
        }
    }
}