using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.Assignments;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Assignments;

public static class CreateAssignment
{
    public class Query : IRequest<Response>
    {
        public AuthorizedUser User { get; }
        public AssignmentCreateArguments AssignmentCreateArguments { get; }

        public Query(AuthorizedUser user, AssignmentCreateArguments assignmentCreateArguments)
        {
            User = user;
            AssignmentCreateArguments = assignmentCreateArguments;
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
        private readonly IMapper _mapper;
        private readonly IwentysDbContext _context;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient, IMapper mapper)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            Student author = await _entityManagerApiClient.StudentProfiles.GetByIdAsync(request.User.Id);
            SubjectProfileDto subject = await _entityManagerApiClient.Client.Subjects.GetSubjectByIdAsync(request.AssignmentCreateArguments.SubjectId);

            if (!request.AssignmentCreateArguments.ForStudyGroup)
            {
                var studentAssignment = StudentAssignment.CreateSingle(author, request.AssignmentCreateArguments);
                _context.StudentAssignments.Add(studentAssignment);
                return new Response(new List<AssignmentInfoDto> {new AssignmentInfoDto(studentAssignment, subject)});
            }

            if (author.GroupId is null)
                throw InnerLogicException.StudyExceptions.UserIsNotGroupAdmin(author.Id);

            StudyGroupProfileResponseDto studyGroup = await _entityManagerApiClient.Client.StudyGroups.GetByStudentIdAsync(author.Id);
            if (studyGroup.GroupAdminId != author.Id)
                throw InnerLogicException.StudyExceptions.UserIsNotGroupAdmin(author.Id);

            List<Student> students = studyGroup
                .Students
                .Select(s => _mapper.Map<Student>(s))
                .ToList();

            List<StudentAssignment> assignments = StudentAssignment.Create(author, request.AssignmentCreateArguments, students);
            _context.StudentAssignments.AddRange(assignments);

            return new Response(assignments.Select(a => new AssignmentInfoDto(a, subject)).ToList());
        }
    }
}