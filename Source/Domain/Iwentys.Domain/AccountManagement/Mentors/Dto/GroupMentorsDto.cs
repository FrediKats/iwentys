using System.Collections.Generic;

namespace Iwentys.Domain.AccountManagement.Mentors.Dto
{
    public class GroupMentorsDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public MentorDto LectorMentor { get; set; }
        public IReadOnlyList<MentorDto> PracticeMentors { get; set; }

        public GroupMentorsDto()
        {
        }

        public GroupMentorsDto(int id, string name, MentorDto lectorMentor, IReadOnlyList<MentorDto> practiceMentors)
        {
            Id = id;
            GroupName = name;
            LectorMentor = lectorMentor;
            PracticeMentors = practiceMentors;
        }
    }
}