using Iwentys.Models.Types;

namespace Iwentys.Models.Entities
{
    public class CompanyWorker
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int WorkerId { get; set; }
        public UserProfile Worker { get; set; }

        public CompanyWorkerType Type { get; set; }

        public static CompanyWorker NewRequest(Company company, UserProfile worker)
        {
            return new CompanyWorker
            {
                Company = company,
                Worker = worker,
                Type = CompanyWorkerType.Requested
            };
        }
    }
}