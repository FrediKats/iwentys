using System.Linq;
using Bogus;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Database.Seeding.FakerEntities.Study;
using Iwentys.Domain;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Enums;

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


            _context.UnitOfWork.GetRepository<StudyGroup>().InsertAsync(studyGroup).Wait();
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

            _context.UnitOfWork.GetRepository<Subject>().InsertAsync(subject).Wait();
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
            _context.UnitOfWork.GetRepository<GroupSubject>().InsertAsync(groupSubject).Wait();
            _context.UnitOfWork.CommitAsync().Wait();
            return groupSubject;
        }

        public SubjectAssignmentDto WithSubjectAssignment(AuthorizedUser user, GroupSubject groupSubject)
        {
            return _context.SubjectAssignmentService.CreateSubjectAssignment(user, groupSubject.SubjectId, SubjectAssignmentFaker.Instance.CreateSubjectAssignmentCreateArguments()).Result;
        }

        public SubjectAssignmentSubmitDto WithSubjectAssignmentSubmit(AuthorizedUser user, SubjectAssignmentDto assignment)
        {
            return _context.SubjectAssignmentService.SendSubmit(user, assignment.Id, SubjectAssignmentFaker.Instance.CreateSubjectAssignmentSubmitCreateArguments()).Result;
        }

        public void WithSubjectAssignmentSubmitFeedback(AuthorizedUser user, SubjectAssignmentSubmitDto submit, FeedbackType feedbackType = FeedbackType.Approve)
        {
            _context.SubjectAssignmentService.SendFeedback(user, submit.Id, SubjectAssignmentFaker.Instance.CreateFeedback(feedbackType)).Wait();
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