using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
    public class CreateSubjectAssignment
    {
        public class Query : IRequest<Response>
        {
            public Query(AssignmentCreateArguments arguments, AuthorizedUser authorizedUser)
            {
                Arguments = arguments;
                AuthorizedUser = authorizedUser;
            }

            public AssignmentCreateArguments Arguments { get; set; }
            public AuthorizedUser AuthorizedUser { get; set; }
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

            //TODO: here must be subject id and we need to resolve group subject
            //TODO: OR list of group subject ids
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Subject subject = await _subjectRepository.GetById(request.Arguments.SubjectId);
                IwentysUser creator = await _iwentysUserRepository.GetById(request.AuthorizedUser.Id);

                var subjectAssignment = SubjectAssignment.Create(creator, subject, request.Arguments);

                _subjectAssignmentRepository.Insert(subjectAssignment);
                _studentAssignmentRepository.Insert(subjectAssignment.StudentAssignments);

                await _unitOfWork.CommitAsync();
                
                SubjectAssignmentDto result = await _subjectAssignmentRepository
                    .Get()
                    .Where(sa => sa.Id == subjectAssignment.Id)
                    .Select(SubjectAssignmentDto.FromEntity)
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}