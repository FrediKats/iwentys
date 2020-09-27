using Iwentys.Models.Entities.Github;

namespace Iwentys.Models.Transferable.Students
{
    public class CodingActivityInfoDto
    {
        public string Month { get; set; }
        public int Activity { get; set; }

        public static CodingActivityInfoDto Wrap(ContributionsInfo contributions)
        {
            return new CodingActivityInfoDto
            {
                Month = contributions.Date,
                Activity = contributions.Count
            };
        }
    }
}