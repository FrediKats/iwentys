using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Study.Study.Dtos;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Study
{
    public class UpdateGroupSubjectTable
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, int studyGroupId, int subjectId, string tableLink)
            {
                AuthorizedUser = authorizedUser;
                StudyGroupId = studyGroupId;
                SubjectId = subjectId;
                TableLink = tableLink;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
            public int StudyGroupId { get; set; }
            public int SubjectId { get; set; }
            public string TableLink { get; set; }
        }

        public class Response
        {
            public GroupSubjectInfoDto Result { get; }

            public Response(GroupSubjectInfoDto result)
            {
                Result = result;
            }
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
                var groupSubject = _context
                    .GroupSubjects
                    .FirstOrDefault(gs => gs.StudyGroupId == request.StudyGroupId
                                          && gs.SubjectId == request.SubjectId);

                if (groupSubject == null)
                {
                    throw EntityNotFoundException.Create(typeof(GroupSubject), request.SubjectId, request.StudyGroupId);
                }

                groupSubject.TableLink = request.TableLink;

                _context.GroupSubjects.Update(groupSubject);

                await _context.SaveChangesAsync();

                return new Response(new GroupSubjectInfoDto(groupSubject));
            }
        }
    }
}