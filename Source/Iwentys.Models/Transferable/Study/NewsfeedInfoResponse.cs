using System;
using Iwentys.Models.Entities.Newsfeeds;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Models.Transferable.Study
{
    public class NewsfeedInfoResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public string SourceLink { get; set; }

        public StudentPartialProfileDto Author { get; set; }

        public static NewsfeedInfoResponse Wrap(NewsfeedEntity entity)
        {
            return new NewsfeedInfoResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                CreationTimeUtc = entity.CreationTimeUtc,
                SourceLink = entity.SourceLink,
                Author = new StudentPartialProfileDto(entity.Author)
            };
        }
    }
}