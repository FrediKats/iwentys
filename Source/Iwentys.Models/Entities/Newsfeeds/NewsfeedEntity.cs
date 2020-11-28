using System;

namespace Iwentys.Models.Entities.Newsfeeds
{
    public class NewsfeedEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public string SourceLink { get; set; }

        public int AuthorId { get; set; }
        public StudentEntity Author { get; set; }
    }
}