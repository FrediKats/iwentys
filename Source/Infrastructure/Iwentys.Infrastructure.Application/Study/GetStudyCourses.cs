using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Study
{
    public class GetStudyCourses
    {
        public class Query : IRequest<Response>
        {
        }

        public class Response
        {
            public Response(List<StudyCourseInfoDto> courses)
            {
                Courses = courses;
            }

            public List<StudyCourseInfoDto> Courses { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<StudyCourse> _studyCourseRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _studyCourseRepository = _unitOfWork.GetRepository<StudyCourse>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                List<StudyCourseInfoDto> result = await _studyCourseRepository
                    .Get()
                    .Select(StudyCourseInfoDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}