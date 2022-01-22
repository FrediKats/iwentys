using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.SubjectAssignments;

public class GetMentorSubjectAssignments
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser user)
        {
            User = user;
        }

        public AuthorizedUser User { get; set; }
    }

    public class Response
    {
        public Response(List<SubjectAssignmentJournalItemDto> subjectAssignments)
        {
            SubjectAssignments = subjectAssignments;
        }

        public List<SubjectAssignmentJournalItemDto> SubjectAssignments { get; set; }
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly IMapper _mapper;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;


        public Handler(IwentysDbContext context, IMapper mapper, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _mapper = mapper;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id);

            if (user.IsAdmin)
            {
                List<SubjectAssignmentJournalItemDto> assignments = await _context
                    .Subjects
                    .ProjectTo<SubjectAssignmentJournalItemDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new Response(assignments);
            }
            else
            {
                //TODO: optimization?
                IReadOnlyCollection<SubjectTeachersDto> subjectTeachers = await _entityManagerApiClient.Teachers.Client.GetAllAsync();
                HashSet<int> allowedSubject = subjectTeachers
                    .Where(st => st.GroupTeachers.Any(gt => gt.Teachers.Any(t => t.TeacherType != TeacherType.None && t.Id == request.User.Id)))
                    .Select(st => st.SubjectId)
                    .ToHashSet();

                Expression<Func<Subject, bool>> isAllowedSubject = (s) => allowedSubject.Contains(s.Id);

                List<SubjectAssignmentJournalItemDto> assignments = await _context
                    .Subjects
                    .Where(isAllowedSubject)
                    .ProjectTo<SubjectAssignmentJournalItemDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new Response(assignments);
            }
        }
    }
}