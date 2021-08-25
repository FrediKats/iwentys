using System.Collections.Generic;

namespace Iwentys.Infrastructure.Application.Modules.AccountManagment.Dtos
{
    public class GroupMentorsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MentorDto LectorMentor { get; set; }
        public IReadOnlyList<MentorDto> PracticeMentors { get; set; }

        public GroupMentorsDto()
        {
        }

        public GroupMentorsDto(int id, string name, MentorDto lectorMentor, IReadOnlyList<MentorDto> practiceMentors)
        {
            Id = id;
            Name = name;
            LectorMentor = lectorMentor;
            PracticeMentors = practiceMentors;
        }
    }
}