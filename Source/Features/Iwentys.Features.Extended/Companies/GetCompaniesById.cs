using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Companies;
using Iwentys.Domain.Companies.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.Companies
{
    public class GetCompaniesById
    {
        public class Query : IRequest<Response>
        {
            public int CompanyId { get; set; }

            public Query(int companyId)
            {
                CompanyId = companyId;
            }
        }

        public class Response
        {
            public Response(CompanyInfoDto company)
            {
                Company = company;
            }

            public CompanyInfoDto Company { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<Company> _companyRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _companyRepository = _unitOfWork.GetRepository<Company>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _companyRepository
                    .Get()
                    .Where(c => c.Id == request.CompanyId)
                    .Select(entity => new CompanyInfoDto(entity))
                    .FirstAsync();

                return new Response(result);
            }
        }
    }
}