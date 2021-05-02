using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Study
{
    public class SearchSubjects
    {
        public class Query : IRequest<Response>
        {
            public StudySearchParametersDto SearchParametersDto { get; set; }

            public Query(StudySearchParametersDto searchParametersDto)
            {
                SearchParametersDto = searchParametersDto;
            }
        }

        public class Response
        {
            public Response(List<SubjectProfileDto> subjects)
            {
                Subjects = subjects;
            }

            public List<SubjectProfileDto> Subjects { get; set; }
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
                List<SubjectProfileDto> result = await _groupSubjectRepository
                    .Get()
                    .SearchSubjects(request.SearchParametersDto)
                    .Select(entity => new SubjectProfileDto(entity))
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}