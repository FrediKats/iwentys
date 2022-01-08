using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Assignments
{
    public static class GetStudentAssignment
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; }

            public Query(AuthorizedUser user)
            {
                User = user;
            }
        }

        public class Response
        {
            public Response(List<AssignmentInfoDto> assignmentInfos)
            {
                AssignmentInfos = assignmentInfos;
            }

            public List<AssignmentInfoDto> AssignmentInfos { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser iwentysUser = await _context.IwentysUsers.GetById(request.User.Id);
                if (iwentysUser.IsAdmin)
                {
                    List<AssignmentInfoDto> result = await _context
                        .StudentAssignments
                        .Select(AssignmentInfoDto.FromStudentEntity)
                        .ToListAsync();

                    return new Response(result);
                }
                else
                {
                    List<AssignmentInfoDto> result = await _context
                    .StudentAssignments
                    .Where(a => a.StudentId == request.User.Id)
                    .Select(AssignmentInfoDto.FromStudentEntity)
                    .ToListAsync();

                    return new Response(result);
                }
            }
        }
    }
}