using System;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class Newsfeed
    {
        //TODO: implement comments and reaction
        public int Id { get; init; }
        public string Title { get; init; }
        public string Content { get; init; }
        public DateTime CreationTimeUtc { get; init; }
        public string SourceLink { get; init; }

        //TODO: replace Student with iwentys user
        public int AuthorId { get; init; }
        public virtual Student Author { get; init; }
    }
}