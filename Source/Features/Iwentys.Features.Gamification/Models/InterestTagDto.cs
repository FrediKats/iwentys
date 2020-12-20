using Iwentys.Features.Gamification.Entities;

namespace Iwentys.Features.Gamification.Models
{
    public class InterestTagDto
    {
        public string Title { get; set; }

        public InterestTagDto(InterestTagEntity interestTag) : this()
        {
            Title = interestTag.Title;
        }

        public InterestTagDto()
        {
        }
    }
}