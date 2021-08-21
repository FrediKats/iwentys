using System;
using System.Linq.Expressions;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.Application.Controllers.Study.Dtos;

namespace Iwentys.Modules.Study.Study.Dtos
{
    public class GroupSubjectInfoDto
    {
        public SubjectProfileDto Subject { get; init; }

        public GroupProfileResponsePreviewDto StudyGroup { get; init; }

        public string Table { get; set; }

        public GroupSubjectInfoDto(GroupSubject entity)
            : this(new SubjectProfileDto(entity.Subject), new GroupProfileResponsePreviewDto(entity.StudyGroup), entity.Table)
        {
        }

        public GroupSubjectInfoDto(SubjectProfileDto subject, GroupProfileResponsePreviewDto studyGroup, string table)
        {
            Subject = subject;
            StudyGroup = studyGroup;
            Table = table;
        }

        public GroupSubjectInfoDto()
        {
        }
        
        public static Expression<Func<GroupSubject, GroupSubjectInfoDto>> FromEntity =>
            entity => new GroupSubjectInfoDto
            {
                Subject = new SubjectProfileDto(entity.Subject),
                StudyGroup = new GroupProfileResponsePreviewDto(entity.StudyGroup),
                Table = entity.Table,
            };
    }
}