using System;
using System.Linq.Expressions;
using Iwentys.Domain.Study;

namespace Iwentys.Study;

public class GroupSubjectInfoDto
{
    public SubjectProfileDto Subject { get; init; }

    public GroupProfileResponsePreviewDto StudyGroup { get; init; }

    public string TableLink { get; set; }

    public GroupSubjectInfoDto(GroupSubject entity)
        : this(new SubjectProfileDto(entity.Subject), new GroupProfileResponsePreviewDto(entity.StudyGroup), entity.TableLink)
    {
    }

    public GroupSubjectInfoDto(SubjectProfileDto subject, GroupProfileResponsePreviewDto studyGroup, string tableLink)
    {
        Subject = subject;
        StudyGroup = studyGroup;
        TableLink = tableLink;
    }

    public GroupSubjectInfoDto()
    {
    }
        
    public static Expression<Func<GroupSubject, GroupSubjectInfoDto>> FromEntity =>
        entity => new GroupSubjectInfoDto
        {
            Subject = new SubjectProfileDto(entity.Subject),
            StudyGroup = new GroupProfileResponsePreviewDto(entity.StudyGroup),
            TableLink = entity.TableLink,
        };
}