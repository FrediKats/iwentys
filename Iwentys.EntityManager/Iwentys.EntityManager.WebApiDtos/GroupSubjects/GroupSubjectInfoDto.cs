namespace Iwentys.EntityManager.WebApiDtos;

public class GroupSubjectInfoDto
{
    public SubjectProfileDto Subject { get; init; }

    public StudyGroupProfileResponsePreviewDto StudyGroup { get; init; }

    public string TableLink { get; set; }
}