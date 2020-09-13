using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable.Companies
{
    public class CompanyWorkRequestDto
    {
        public CompanyInfoDto Company { get; set; }
        public StudentEntity Worker { get; set; }

        public static CompanyWorkRequestDto Create(CompanyWorker worker)
        {
            if (worker.Type != CompanyWorkerType.Requested)
                throw new InnerLogicException($"Invalid operation, cannot convert non-request type of {nameof(CompanyWorker)} to {nameof(CompanyWorkRequestDto)}");

            return new CompanyWorkRequestDto
            {
                Company = worker.Company.To(CompanyInfoDto.Create),
                Worker = worker.Worker
            };
        }
    }
}