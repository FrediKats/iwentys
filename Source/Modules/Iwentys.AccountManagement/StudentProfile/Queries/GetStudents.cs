using System.Collections.Generic;
using System.Linq;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.AccountManagement
{
    public static class GetStudents
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
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            protected override Response Handle(Query request)
            {
                List<StudentInfoDto> result = _context
                    .Students
                    .Select(s => new StudentInfoDto(s))
                    .ToList();

                return new Response(result);
            }
        }
    }
}