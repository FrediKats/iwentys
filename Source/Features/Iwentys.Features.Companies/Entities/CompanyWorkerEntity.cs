using Iwentys.Features.Companies.Enums;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Companies.Entities
{
    public class CompanyWorkerEntity
    {
        public int CompanyId { get; set; }
        public CompanyEntity CompanyEntity { get; set; }

        public int WorkerId { get; set; }
        public StudentEntity Worker { get; set; }

        public CompanyWorkerType Type { get; set; }

        public static CompanyWorkerEntity NewRequest(CompanyEntity companyEntity, StudentEntity worker)
        {
            return new CompanyWorkerEntity
            {
                CompanyEntity = companyEntity,
                Worker = worker,
                Type = CompanyWorkerType.Requested
            };
        }
    }
}