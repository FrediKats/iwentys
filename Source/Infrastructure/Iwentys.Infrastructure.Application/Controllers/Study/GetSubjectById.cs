using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Study
{
    public class GetSubjectById
    {
        public class Query : IRequest<Response>
        {
            public int SubjectId { get; set; }

            public Query(int subjectId)
            {
                SubjectId = subjectId;
            }
        }

        public class Response
        {
            public Response(SubjectProfileDto subject)
            {
                Subject = subject;
            }

            public SubjectProfileDto Subject { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<GroupSubject> _groupSubjectRepository;

            private readonly IGenericRepository<Subject> _subjectRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _subjectRepository = _unitOfWork.GetRepository<Subject>();
                _groupSubjectRepository = _unitOfWork.GetRepository<GroupSubject>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                SubjectProfileDto result = await _subjectRepository
                    .Get()
                    .FirstAsync(s => s.Id == request.SubjectId)
                    .To(entity => new SubjectProfileDto(entity));

                return new Response(result);
            }
        }
    }
}