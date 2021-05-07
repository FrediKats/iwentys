using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
    public static class SendFeedback
    {
        public class Query : IRequest<Response>
        {
            public Query(SubjectAssignmentSubmitFeedbackArguments arguments, AuthorizedUser authorizedUser)
            {
                Arguments = arguments;
                AuthorizedUser = authorizedUser;
            }

            public SubjectAssignmentSubmitFeedbackArguments Arguments { get; set; }
            public AuthorizedUser AuthorizedUser { get; set; }
        }

        public class Response
        {
        }

        public class Handler : IRequestHandler<Query, Response>
        {
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
                _subjectRepository = _unitOfWork.GetRepository<Subject>();
                _assignmentRepository = _unitOfWork.GetRepository<Assignment>();
                _studentAssignmentRepository = _unitOfWork.GetRepository<StudentAssignment>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                SubjectAssignmentSubmit subjectAssignmentSubmit = await _subjectAssignmentSubmitRepository.GetById(request.Arguments.SubjectAssignmentSubmitId);
                IwentysUser iwentysUser = await _iwentysUserRepository.GetById(request.AuthorizedUser.Id);

                subjectAssignmentSubmit.ApplyFeedback(iwentysUser, request.Arguments);

                _subjectAssignmentSubmitRepository.Update(subjectAssignmentSubmit);
                await _unitOfWork.CommitAsync();

                return new Response();
            }
        }
    }
}