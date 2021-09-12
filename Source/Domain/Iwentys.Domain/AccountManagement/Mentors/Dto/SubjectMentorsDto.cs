﻿using System.Collections.Generic;

namespace Iwentys.Domain.AccountManagement.Mentors.Dto
{
    public record SubjectMentorsDto
    {
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