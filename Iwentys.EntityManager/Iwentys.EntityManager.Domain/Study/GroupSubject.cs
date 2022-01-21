using Iwentys.EntityManager.Common;
using Iwentys.EntityManager.Domain.Accounts;
using Iwentys.EntityManager.PublicTypes;

namespace Iwentys.EntityManager.Domain;

public class GroupSubject
{
    public int Id { get; init; }

    public int SubjectId { get; init; }
    public virtual Subject Subject { get; init; }
    public StudySemester StudySemester { get; init; }

    public int StudyGroupId { get; init; }
    public virtual StudyGroup StudyGroup { get; init; }

    public virtual List<GroupSubjectMentor> Mentors { get; init; }

    public int? PracticeMentorId { get; init; }
    public virtual UniversitySystemUser PracticeMentor { get; init; }
        
    public virtual string TableLink { get; set; }

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

    public bool HasMentorPermission(IwentysUser user)
    {
        return Mentors.Any(pm=>pm.UserId == user.Id);
    }
}