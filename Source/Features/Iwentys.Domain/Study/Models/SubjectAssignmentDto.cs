using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Domain.AccountManagement.Dto;

namespace Iwentys.Domain.Study.Models
{
    public class SubjectAssignmentDto
    {
        public int Id { get; set; }

        public int GroupSubjectId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IwentysUserInfoDto Author { get; set; }

        public List<SubjectAssignmentSubmitDto> Submits { get; set; }

        public static Expression<Func<SubjectAssignment, SubjectAssignmentDto>> FromEntity =>
            entity => new SubjectAssignmentDto
            {
                Id = entity.Id,
                Title = entity.Assignment.Title,
                Description = entity.Assignment.Description,
                Link = entity.Assignment.Link,
                Author = new IwentysUserInfoDto(entity.Author),
                Submits = entity.SubjectAssignmentSubmits.Select(s => new SubjectAssignmentSubmitDto(s)).ToList()
            };
    }
}