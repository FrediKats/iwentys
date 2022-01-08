using System;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Karmas;

public class KarmaUpVote
{
    public int AuthorId { get; set; }
    public virtual IwentysUser Author { get; set; }

    public int TargetId { get; set; }
    public virtual IwentysUser Target { get; set; }

    public DateTime CreationTimeUtc { get; set; }

    public static KarmaUpVote Create(IwentysUser author, IwentysUser target)
    {
        return new KarmaUpVote
        {
            AuthorId = author.Id,
            TargetId = target.Id,
            CreationTimeUtc = DateTime.UtcNow
        };
    }
}