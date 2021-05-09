using System.Linq;
using Bogus;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Enums;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Study;

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

        public GroupSubject WithGroupSubject(GroupProfileResponseDto studyGroup, Subject subject, IwentysUser teacher = null)
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

        public SubjectAssignment WithSubjectAssignment(IwentysUser creator, GroupSubject groupSubject)
        {
            AssignmentCreateArguments arguments = SubjectAssignmentFaker.Instance.CreateSubjectAssignmentCreateArguments().ConvertToAssignmentCreateArguments(groupSubject.SubjectId);
            var subjectAssignment = SubjectAssignment.Create(creator, groupSubject.Subject, arguments);
            return subjectAssignment;
        }

        public SubjectAssignmentSubmit WithSubjectAssignmentSubmit(IwentysUser user, SubjectAssignment assignment)
        {
            SubjectAssignmentSubmitCreateArguments arguments = SubjectAssignmentFaker.Instance.CreateSubjectAssignmentSubmitCreateArguments(assignment.Id);
            SubjectAssignmentSubmit subjectAssignmentSubmit = assignment.CreateSubmit(user, arguments);
            return subjectAssignmentSubmit;
        }

        public void WithSubjectAssignmentSubmitFeedback(IwentysUser user, SubjectAssignmentSubmit submit, FeedbackType feedbackType = FeedbackType.Approve)
        {
            SubjectAssignmentSubmitFeedbackArguments arguments = SubjectAssignmentFaker.Instance.CreateFeedback(submit.Id, feedbackType);
            submit.ApplyFeedback(user, arguments);
        }

        public AuthorizedUser WithNewStudent(GroupProfileResponseDto studyGroup)
        {
            Student student = WithNewStudentAsStudent(studyGroup);
            AuthorizedUser user = AuthorizedUser.DebugAuth(student.Id);
            return user;
        }

        public Student WithNewStudentAsStudent(GroupProfileResponseDto studyGroup)
        {
            StudentCreateArguments createArguments = UsersFaker.Instance.Students.Generate();
            createArguments.Id = UsersFaker.Instance.GetIdentifier();
            createArguments.GroupId = studyGroup.Id;

            var student = Student.Create(createArguments);
            _context.UnitOfWork.GetRepository<Student>().Insert(student);
            _context.UnitOfWork.CommitAsync().Wait();

            return student;
        }

        public Student WithNewStudentAsStudent(StudyGroup studyGroup)
        {
            StudentCreateArguments createArguments = UsersFaker.Instance.Students.Generate();
            createArguments.Id = UsersFaker.Instance.GetIdentifier();
            createArguments.GroupId = studyGroup.Id;

            var student = Student.Create(createArguments);
            studyGroup.AddStudent(student);
            return student;
        }
    }
}