using System.Collections.Generic;

namespace Iwentys.Domain.AccountManagement.Mentors.Dto
{
    public class GroupMentorsDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public IReadOnlyList<MentorDto> LectorMentors { get; set; }
        public IReadOnlyList<MentorDto> PracticeMentors { get; set; }

        public GroupMentorsDto()
        {
        }

        public GroupMentorsDto(int id, string name, IReadOnlyList<MentorDto> lectorMentors, IReadOnlyList<MentorDto> practiceMentors)
        {
            Id = id;
            GroupName = name;
            LectorMentors = lectorMentors;
            PracticeMentors = practiceMentors;
        }
    }
}