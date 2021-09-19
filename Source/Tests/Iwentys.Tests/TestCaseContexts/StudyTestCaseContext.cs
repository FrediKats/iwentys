using System.Linq;
using Bogus;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Enums;
using Iwentys.Domain.Study.Models;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Domain.SubjectAssignments.Enums;
using Iwentys.Domain.SubjectAssignments.Models;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Study;
using Iwentys.Modules.Study.Study.Dtos;

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

            _context._context.StudyGroups.Add(studyGroup);
            _context._context.SaveChanges();

            return _context._context
                .StudyGroups
                .Where(sg => sg.Id == studyGroup.Id)
                .Select(GroupProfileResponseDto.FromEntity)
                .Single();
        }

        public Subject WithSubject()
        {
            var subject = new Subject
            {
                Title = new Faker().Lorem.Word()
            };

            _context._context.Subjects.Add(subject);
            _context._context.SaveChanges();
            return subject;
        }

        public GroupSubject WithGroupSubject(GroupProfileResponseDto studyGroup, Subject subject, IwentysUser teacher = null)
        {
            var groupSubject = new GroupSubject
            {
                StudyGroupId = studyGroup.Id,
                SubjectId = subject.Id,
                StudySemester = StudySemester.Y21H1,
                // LectorMentorId = teacher?.Id
            };

            _context._context.GroupSubjects.Add(groupSubject);
            _context._context.SaveChanges();
            return groupSubject;
        }

        public SubjectAssignment WithSubjectAssignment(IwentysUser creator, GroupSubject groupSubject)
        {
            var arguments = SubjectAssignmentFaker.Instance.CreateSubjectAssignmentCreateArguments(groupSubject.SubjectId);
            var subjectAssignment = SubjectAssignment.Create(creator, groupSubject.Subject, arguments);
            return subjectAssignment;
        }

        public void WithSubjectAssignmentSubmitFeedback(IwentysUser user, SubjectAssignmentSubmit submit, FeedbackType feedbackType = FeedbackType.Approve)
        {
            SubjectAssignmentSubmitFeedbackArguments arguments = SubjectAssignmentFaker.Instance.CreateFeedback(submit.Id, feedbackType);
            submit.AddFeedback(user, arguments);
        }

        public Student WithNewStudentAsStudent(GroupProfileResponseDto studyGroup)
        {
            StudentCreateArguments createArguments = UsersFaker.Instance.Students.Generate();
            createArguments.Id = UsersFaker.Instance.GetIdentifier();
            createArguments.GroupId = studyGroup.Id;

            var student = Student.Create(createArguments);
            _context._context.Students.Add(student);
            _context._context.SaveChanges();

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