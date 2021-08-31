using System.Collections.Generic;

namespace Iwentys.Domain.AccountManagement.Mentors.Dto
{
    public class GroupMentorsDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public IReadOnlyList<MentorDto> Mentors { get; set; }

        public GroupMentorsDto()
        {
        }

        public GroupMentorsDto(int id, string name, IReadOnlyList<MentorDto> lectorMentors, IReadOnlyList<MentorDto> Mentors)
        {
            Id = id;
            GroupName = name;
            Mentors = lectorMentors;
        }
    }
}