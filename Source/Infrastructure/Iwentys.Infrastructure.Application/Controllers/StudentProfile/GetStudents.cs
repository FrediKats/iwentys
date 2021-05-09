using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.StudentProfile
{
    public class GetStudents
    {
        public class Query : IRequest<Response>
        {
        }

        public class Response
        {
            public Response(List<StudentInfoDto> students)
            {
                Students = students;
            }

            public List<StudentInfoDto> Students { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IGenericRepository<Student> _studentRepository;

            public Handler(IUnitOfWork unitOfWork)
            {
                _studentRepository = unitOfWork.GetRepository<Student>();
            }

            protected override Response Handle(Query request)
            {
                List<StudentInfoDto> result = _studentRepository
                    .Get()
                    .Select(s => new StudentInfoDto(s))
                    .ToList();

                return new Response(result);
            }
        }
    }
}