using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.AccountManagement
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

            public Handler(IwentysDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
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