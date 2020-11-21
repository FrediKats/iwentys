using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities;
using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable.Companies
{
    public class CompanyWorkRequestDto
    {
        public CompanyInfoResponse Company { get; set; }
        public StudentEntity Worker { get; set; }

        public static CompanyWorkRequestDto Create(CompanyWorkerEntity workerEntity)
        {
            if (workerEntity.Type != CompanyWorkerType.Requested)
                throw new InnerLogicException($"Invalid operation, cannot convert non-request type of {nameof(CompanyWorkerEntity)} to {nameof(CompanyWorkRequestDto)}");

            return new CompanyWorkRequestDto
            {
                Company = workerEntity.CompanyEntity.To(CompanyInfoResponse.Create),
                Worker = workerEntity.Worker
            };
        }
    }
}