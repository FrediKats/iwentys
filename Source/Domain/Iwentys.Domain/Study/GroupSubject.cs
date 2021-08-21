using System;
using System.Text.Json;
using FluentResults;
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

        public int? LectorMentorId { get; init; }
        public virtual UniversitySystemUser LectorMentor { get; init; }

        public int? PracticeMentorId { get; init; }
        public virtual UniversitySystemUser PracticeMentor { get; init; }
        
        public virtual string Table { get; set; }

        public GroupSubject()
        {
        }

        //TODO: enable nullability
        public GroupSubject(Subject subject, StudyGroup studyGroup, StudySemester studySemester, UniversitySystemUser lectorMentor, UniversitySystemUser practiceMentor)
        {
            Subject = subject;
            SubjectId = subject.Id;
            StudyGroup = studyGroup;
            StudyGroupId = studyGroup.Id;
            StudySemester = studySemester;
            LectorMentor = lectorMentor;
            LectorMentorId = lectorMentor?.Id;
            PracticeMentor = practiceMentor;
            PracticeMentorId = practiceMentor?.Id;
        }


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
            return LectorMentorId == user.Id || PracticeMentorId == user.Id;
        }
    }
}