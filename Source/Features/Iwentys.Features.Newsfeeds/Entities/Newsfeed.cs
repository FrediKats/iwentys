using System;
using Iwentys.Features.AccountManagement.Entities;

namespace Iwentys.Features.Newsfeeds.Entities
{
    public class Newsfeed
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Content { get; init; }
        public DateTime CreationTimeUtc { get; init; }
        public string SourceLink { get; init; }

        public int AuthorId { get; init; }
        public virtual IwentysUser Author { get; init; }
    }
}