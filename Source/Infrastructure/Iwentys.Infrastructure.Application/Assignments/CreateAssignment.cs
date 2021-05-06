using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;

namespace Iwentys.Infrastructure.Application.Assignments
{
    public class CreateAssignment
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; }
            public AssignmentCreateArguments AssignmentCreateArguments { get; }

            public Query(AuthorizedUser user, AssignmentCreateArguments assignmentCreateArguments)
            {
                User = user;
                AssignmentCreateArguments = assignmentCreateArguments;
            }
        }

        public class Response
        {
            public Response(List<AssignmentInfoDto> assignmentInfos)
            {
                AssignmentInfos = assignmentInfos;
            }

            public List<AssignmentInfoDto> AssignmentInfos { get; set; }
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
                Student author = await _studentRepository.GetById(request.User.Id);

                List<StudentAssignment> assignments = StudentAssignment.Create(author, request.AssignmentCreateArguments);

                _studentAssignmentRepository.Insert(assignments);
                await _unitOfWork.CommitAsync();

                return new Response(assignments.Select(a => new AssignmentInfoDto(a)).ToList());
            }
        }
    }
}