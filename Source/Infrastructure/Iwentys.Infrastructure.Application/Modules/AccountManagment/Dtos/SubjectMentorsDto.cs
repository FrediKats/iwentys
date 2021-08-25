using System.Collections.Generic;

namespace Iwentys.Infrastructure.Application.Modules.AccountManagment.Dtos
{
    public record SubjectMentorsDto
    {
        // int Id, string Name, IReadOnlyList<GroupMentorsDto> Groups   
        public int Id { get; set; }
        public string Name { get; set; }
        public IReadOnlyList<GroupMentorsDto> Groups { get; set; }

        public SubjectMentorsDto()
        {
            
        }
        
        public SubjectMentorsDto(int id, string name, IReadOnlyList<GroupMentorsDto> groups)
        {
            Id = id;
            Name = name;
            Groups = groups;
        }
    }
}