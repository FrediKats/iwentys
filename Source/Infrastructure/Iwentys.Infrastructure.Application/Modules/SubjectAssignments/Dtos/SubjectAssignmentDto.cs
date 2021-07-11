using System;
using System.Collections.Generic;
using Iwentys.Domain.AccountManagement.Dto;
using Iwentys.Domain.SubjectAssignments.Enums;

namespace Iwentys.Infrastructure.Application.Modules.SubjectAssignments.Dtos
{
    public class SubjectAssignmentDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IwentysUserInfoDto Author { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public DateTime LastUpdateTimeUtc { get; set; }
        public DateTime? DeadlineTimeUtc { get; set; }
        public int Position { get; set; }
        public AvailabilityState AvailabilityState { get; set; }

        public List<SubjectAssignmentSubmitDto> Submits { get; set; }
    }
}