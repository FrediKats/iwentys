﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Domain.AccountManagement.Dto;

namespace Iwentys.Domain.SubjectAssignments.Models
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
                Title = entity.Title,
                Description = entity.Description,
                Link = entity.Link,
                Author = new IwentysUserInfoDto(entity.Author),
                Submits = entity.SubjectAssignmentSubmits.Select(s => new SubjectAssignmentSubmitDto(s)).ToList()
            };
    }
}