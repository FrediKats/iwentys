using System;
using System.Linq.Expressions;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Companies.Models
{
    public record CompanyWorkRequestDto
    {
        public static CompanyWorkRequestDto Create(CompanyWorker worker)
        {
            if (worker.Type != CompanyWorkerType.Requested)
                throw new InnerLogicException($"Invalid operation, cannot convert non-request type of {nameof(CompanyWorker)} to {nameof(CompanyWorkRequestDto)}");

            return new CompanyWorkRequestDto(new CompanyInfoDto(worker.Company), worker.Worker);
        }

        public CompanyWorkRequestDto(CompanyInfoDto company, Student worker)
        {
            Company = company;
            Worker = worker;
        }

        public CompanyWorkRequestDto()
        {
        }

        public static Expression<Func<CompanyWorker, CompanyWorkRequestDto>> FromEntity => entity => Create(entity);
        
        public CompanyInfoDto Company { get; init; }
        public Student Worker { get; init; }
    }
}