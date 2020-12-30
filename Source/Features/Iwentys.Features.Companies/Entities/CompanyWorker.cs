using System;
using System.Linq.Expressions;
using Iwentys.Features.Companies.Enums;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Companies.Entities
{
    public class CompanyWorker
    {
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int WorkerId { get; set; }
        public virtual Student Worker { get; set; }

        public CompanyWorkerType Type { get; set; }
        public int? ApprovedById { get; set; }
        public virtual Student ApprovedBy { get; set; }

        public static CompanyWorker NewRequest(Company company, Student worker)
        {
            return new CompanyWorker
            {
                Company = company,
                Worker = worker,
                Type = CompanyWorkerType.Requested
            };
        }
        
        public void Approve(SystemAdminUser systemAdminUser)
        {
            ApprovedById = systemAdminUser.Student.Id;
            Type = CompanyWorkerType.Accepted;
        }
        
        public static Expression<Func<CompanyWorker, bool>> IsRequested => worker => worker.Type == CompanyWorkerType.Requested;
    }
}