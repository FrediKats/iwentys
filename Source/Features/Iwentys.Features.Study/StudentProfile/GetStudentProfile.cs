using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study;
using MediatR;

namespace Iwentys.Features.Study.StudentProfile
{
    public class GetStudentProfile
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
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _studentRepository = _unitOfWork.GetRepository<Student>();
            }

            protected override Response Handle(Query request)
            {
                List<Student> students = _studentRepository
                    .Get()
                    .ToList();

                return new Response(students.SelectToList(s => new StudentInfoDto(s)));
            }
        }
    }
}