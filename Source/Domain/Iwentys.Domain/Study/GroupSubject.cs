using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FluentResults;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study.Enums;

namespace Iwentys.Domain.Study
{
    public class GroupSubject
    {
        public int Id { get; init; }

        public int SubjectId { get; init; }
        public virtual Subject Subject { get; init; }
        public StudySemester StudySemester { get; init; }

        public int StudyGroupId { get; init; }
        public virtual StudyGroup StudyGroup { get; init; }

        public virtual List<GroupSubjectMentor> Mentors { get; init; }

        public GroupSubject()
        {
        }

        //TODO: enable nullability
        public GroupSubject(Subject subject, StudyGroup studyGroup, StudySemester studySemester, IwentysUser lectorMentor)
        {
            Subject = subject;
            SubjectId = subject.Id;
            StudyGroup = studyGroup;
            StudyGroupId = studyGroup.Id;
            StudySemester = studySemester;
            Mentors = new List<GroupSubjectMentor>()
            {
                new GroupSubjectMentor()
                {
                    IsLector = true,
                    User = lectorMentor
                }
            };
        }

        public void AddPracticeMentor(IwentysUser practiceMentor)
        {
            if (!IsPracticeMentor(practiceMentor))
            {
                throw new IwentysException("User is already practice mentor");
            }

            Mentors.Add(new GroupSubjectMentor()
                            {
                                GroupSubjectId = Id,
                                UserId = practiceMentor.Id
                            });
        }

        private bool IsPracticeMentor(IwentysUser mentor)
            => Mentors.All(pm => !pm.IsLector || pm.UserId != mentor.Id);

        public string SerializedGoogleTableConfig { get; set; }

        public Result<GoogleTableData> TryGetGoogleTableDataConfig()
        {
            if (SerializedGoogleTableConfig is null)
                return Result.Fail<GoogleTableData>("Value is not set");

            try
            {
                var googleTableData = JsonSerializer.Deserialize<GoogleTableData>(SerializedGoogleTableConfig);
                return Result.Ok(googleTableData);
            }
            catch (Exception e)
            {
                return Result.Fail<GoogleTableData>(new Error("Data parse failed").CausedBy(e));
            }
        }

        public bool HasMentorPermission(IwentysUser user)
        {
            return Mentors.Any(pm=>pm.UserId == user.Id);
        }
    }
}