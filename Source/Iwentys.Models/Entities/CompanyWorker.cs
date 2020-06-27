namespace Iwentys.Models.Entities
{
    public class CompanyWorker
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int WorkerId { get; set; }
        public UserProfile Worker { get; set; }
    }
}