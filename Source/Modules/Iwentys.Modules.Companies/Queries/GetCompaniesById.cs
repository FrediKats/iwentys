using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Companies.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.Companies.Queries
{
    public static class GetCompaniesById
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
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }


            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                CompanyInfoDto result = await _context
                    .Companies
                    .Where(c => c.Id == request.CompanyId)
                    .Select(entity => new CompanyInfoDto(entity))
                    .FirstAsync();

                return new Response(result);
            }
        }
    }
}