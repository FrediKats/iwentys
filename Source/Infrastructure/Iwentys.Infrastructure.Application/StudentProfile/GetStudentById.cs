using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.Common.Databases;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.StudentProfile
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
            private readonly IwentysDbContext _context;
            private readonly IMapper _mapper;
            private readonly IGenericRepository<Student> _studentRepository;

            public Handler(IUnitOfWork unitOfWork, IwentysDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
                _studentRepository = unitOfWork.GetRepository<Student>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                StudentInfoDto result = await _mapper
                    .ProjectTo<StudentInfoDto>(_context.Students)
                    .FirstAsync(s => s.Id == request.StudentId);

                return new Response(result);
            }
        }
    }
}