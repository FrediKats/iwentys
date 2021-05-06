using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.SubjectAssignments
{
    public class GetSubjectAssignmentSubmit
    {
        public class Query : IRequest<Response>
        {
            public Query(int subjectAssignmentSubmitId, AuthorizedUser authorizedUser)
            {
                SubjectAssignmentSubmitId = subjectAssignmentSubmitId;
                AuthorizedUser = authorizedUser;
            }

            public int SubjectAssignmentSubmitId { get; set; }
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
            private readonly IGenericRepository<GroupSubjectAssignment> _groupSubjectAssignmentRepository;
            private readonly IGenericRepository<GroupSubject> _groupSubjectRepository;

            private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;
            private readonly IGenericRepository<Assignment> _assignmentRepository;
            private readonly IGenericRepository<StudentAssignment> _studentAssignmentRepository;
            private readonly IGenericRepository<SubjectAssignment> _subjectAssignmentRepository;
            private readonly IGenericRepository<SubjectAssignmentSubmit> _subjectAssignmentSubmitRepository;
            private readonly IGenericRepository<Subject> _subjectRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
                _subjectAssignmentRepository = _unitOfWork.GetRepository<SubjectAssignment>();
                _subjectAssignmentSubmitRepository = _unitOfWork.GetRepository<SubjectAssignmentSubmit>();
                _groupSubjectAssignmentRepository = _unitOfWork.GetRepository<GroupSubjectAssignment>();
                _groupSubjectRepository = _unitOfWork.GetRepository<GroupSubject>();
                _subjectRepository = _unitOfWork.GetRepository<Subject>();
                _assignmentRepository = _unitOfWork.GetRepository<Assignment>();
                _studentAssignmentRepository = _unitOfWork.GetRepository<StudentAssignment>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                SubjectAssignmentSubmitDto result = await _subjectAssignmentSubmitRepository
                    .Get()
                    .Where(sas => sas.Id == request.SubjectAssignmentSubmitId)
                    .Select(sas => new SubjectAssignmentSubmitDto(sas))
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}