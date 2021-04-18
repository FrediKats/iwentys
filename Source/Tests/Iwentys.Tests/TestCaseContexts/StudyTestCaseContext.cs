using System.Linq;
using System.Threading;
using Bogus;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Database.Seeding.FakerEntities.Study;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Enums;
using Iwentys.Domain.Study.Models;
using Iwentys.Features.Study.SubjectAssignments;

namespace Iwentys.Tests.TestCaseContexts
{
    public class StudyTestCaseContext
    {
        private readonly TestCaseContext _context;

        public StudyTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public GroupProfileResponseDto WithStudyGroup()
        {
            var studyGroup = new StudyGroup
            {
                GroupName = new Faker().Lorem.Word()
            };


            _context.UnitOfWork.GetRepository<StudyGroup>().Insert(studyGroup);
            _context.UnitOfWork.CommitAsync().Wait();
            return _context.UnitOfWork
                .GetRepository<StudyGroup>()
                .Get()
                .Where(sg => sg.Id == studyGroup.Id)
                .Select(GroupProfileResponseDto.FromEntity)
                .Single();
        }

        public Subject WithSubject()
        {
            var subject = new Subject
            {
                Name = new Faker().Lorem.Word()
            };

            _context.UnitOfWork.GetRepository<Subject>().Insert(subject);
            _context.UnitOfWork.CommitAsync().Wait();
            return subject;
        }

        public GroupSubject WithGroupSubject(GroupProfileResponseDto studyGroup, Subject subject, AuthorizedUser teacher = null)
        {
            var groupSubject = new GroupSubject
            {
                StudyGroupId = studyGroup.Id,
                SubjectId = subject.Id,
                StudySemester = StudySemester.Y21H1,
                LectorTeacherId = teacher?.Id
            };
            _context.UnitOfWork.GetRepository<GroupSubject>().Insert(groupSubject);
            _context.UnitOfWork.CommitAsync().Wait();
            return groupSubject;
        }

        public SubjectAssignmentDto WithSubjectAssignment(AuthorizedUser user, GroupSubject groupSubject)
        {
            AssignmentCreateArguments arguments = SubjectAssignmentFaker.Instance.CreateSubjectAssignmentCreateArguments().ConvertToAssignmentCreateArguments(groupSubject.SubjectId);
            return new CreateSubjectAssignment.Handler(_context.UnitOfWork).Handle(new CreateSubjectAssignment.Query(arguments, user), CancellationToken.None).Result.SubjectAssignment;
        }

        public SubjectAssignmentSubmitDto WithSubjectAssignmentSubmit(AuthorizedUser user, SubjectAssignmentDto assignment)
        {
            return new SendSubmit.Handler(_context.UnitOfWork).Handle(new SendSubmit.Query(SubjectAssignmentFaker.Instance.CreateSubjectAssignmentSubmitCreateArguments(assignment.Id), user), CancellationToken.None).Result.Submit;
        }

        public void WithSubjectAssignmentSubmitFeedback(AuthorizedUser user, SubjectAssignmentSubmitDto submit, FeedbackType feedbackType = FeedbackType.Approve)
        {
            new SendFeedback.Handler(_context.UnitOfWork).Handle(new SendFeedback.Query(SubjectAssignmentFaker.Instance.CreateFeedback(submit.Id, feedbackType), user), CancellationToken.None).Wait();
        }

        public AuthorizedUser WithNewStudent(GroupProfileResponseDto studyGroup)
        {
            StudentCreateArguments createArguments = UsersFaker.Instance.Students.Generate();
            createArguments.Id = UsersFaker.Instance.GetIdentifier();
            createArguments.GroupId = studyGroup.Id;

            StudentInfoDto studentInfoDto = _context.StudentService.Create(createArguments).Result;

            AuthorizedUser user = AuthorizedUser.DebugAuth(studentInfoDto.Id);
            return user;
        }
    }
}