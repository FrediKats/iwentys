using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.SubjectAssignments
{
    public class GetSubjectAssignmentForSubject
    {
        public class Query : IRequest<Response>
        {
            public Query(int subjectId)
            {
                SubjectId = subjectId;
            }

            public int SubjectId { get; set; }
        }

        public class Response
        {
            public Response(List<SubjectAssignmentDto> subjectAssignments)
            {
                SubjectAssignments = subjectAssignments;
            }

            public List<SubjectAssignmentDto> SubjectAssignments { get; set; }

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

            //TODO: it is not work?
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                List<SubjectAssignmentDto> result = await _subjectAssignmentRepository
                    .Get()
                    .Where(sa => sa.SubjectId == request.SubjectId)
                    .Select(SubjectAssignmentDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}