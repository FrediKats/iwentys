using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;

namespace Iwentys.Features.Study.StudentProfile
{
    public class GetStudentById
    {
        public class Query : IRequest<Response>
        {
            public Query(int studentId)
            {
                StudentId = studentId;
            }

            public int StudentId { get; set; }
        }

        public class Response
        {
            public Response(StudentInfoDto student)
            {
                Student = student;
            }

            public StudentInfoDto Student { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<Student> _studentRepository;

            public Handler(IUnitOfWork unitOfWork)
            {
                _studentRepository = unitOfWork.GetRepository<Student>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                StudentInfoDto result = await _studentRepository
                    .GetById(request.StudentId)
                    .To(s => new StudentInfoDto(s));

                return new Response(result);
            }
        }
    }
}