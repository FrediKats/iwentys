using System;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class Newsfeed
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public string SourceLink { get; set; }

        public int AuthorId { get; set; }
        public virtual Student Author { get; set; }
    }
}