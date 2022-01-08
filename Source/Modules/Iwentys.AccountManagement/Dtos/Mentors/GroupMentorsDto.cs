using System.Collections.Generic;

namespace Iwentys.AccountManagement
{
    public class GroupMentorsDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public IReadOnlyList<MentorDto> Mentors { get; set; }

        public GroupMentorsDto()
        {
        }

        public GroupMentorsDto(int id, string name, IReadOnlyList<MentorDto> mentors)
        {
            Id = id;
            GroupName = name;
            Mentors = mentors;
        }
    }
}