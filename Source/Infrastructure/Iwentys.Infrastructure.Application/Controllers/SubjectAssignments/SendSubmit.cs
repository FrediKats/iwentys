using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
    public class SendSubmit
    {
        public class Query : IRequest<Response>
        {
            public Query(SubjectAssignmentSubmitCreateArguments arguments, AuthorizedUser authorizedUser)
            {
                Arguments = arguments;
                AuthorizedUser = authorizedUser;
            }

            public SubjectAssignmentSubmitCreateArguments Arguments { get; set; }
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
            private readonly IGenericRepository<SubjectAssignment> _subjectAssignmentRepository;
            private readonly IGenericRepository<SubjectAssignmentSubmit> _subjectAssignmentSubmitRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _subjectAssignmentRepository = _unitOfWork.GetRepository<SubjectAssignment>();
                _subjectAssignmentSubmitRepository = _unitOfWork.GetRepository<SubjectAssignmentSubmit>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                SubjectAssignment subjectAssignment = await _subjectAssignmentRepository.GetById(request.Arguments.SubjectAssignmentId);

                SubjectAssignmentSubmit subjectAssignmentSubmit = subjectAssignment.CreateSubmit(request.AuthorizedUser, request.Arguments);

                _subjectAssignmentSubmitRepository.Insert(subjectAssignmentSubmit);
                await _unitOfWork.CommitAsync();

                
                SubjectAssignmentSubmitDto result = await _subjectAssignmentSubmitRepository
                    .Get()
                    .Where(sas => sas.Id == subjectAssignmentSubmit.Id)
                    .Select(sas => new SubjectAssignmentSubmitDto(sas))
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}