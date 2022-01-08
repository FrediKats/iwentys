using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Domain.Karmas
{
    public class KarmaStatistic
    {
        public int StudentId { get; set; }
        public int Karma { get; set; }

        public List<int> UpVotes { get; set; }

        public static KarmaStatistic Create(int studentId, List<KarmaUpVote> upVotes)
        {
            return new KarmaStatistic
            {
                StudentId = studentId,
                Karma = upVotes.Count,
                UpVotes = upVotes.Select(u => u.AuthorId).ToList()
            };
        }
    }
}