using System;
using System.Linq.Expressions;
using Iwentys.Features.Gamification.Entities;

namespace Iwentys.Features.Gamification.Models
{
    public class InterestTagDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public InterestTagDto(InterestTag interestTag) : this()
        {
            Id = interestTag.Id;
            Title = interestTag.Title;
        }

        public InterestTagDto()
        {
        }

        public static Expression<Func<InterestTag, InterestTagDto>> FromEntity => entity => new InterestTagDto(entity);
    }
}