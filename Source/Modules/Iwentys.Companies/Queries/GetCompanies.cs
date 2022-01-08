using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Companies;

public static class GetCompanies
{
    public class Query : IRequest<Response>
    {

    }

    public class Response
    {
        public Response(List<CompanyInfoDto> companies)
        {
            Companies = companies;
        }

        public List<CompanyInfoDto> Companies { get; set; }
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
            List<CompanyInfoDto> result = await _context
                .Companies
                .Select(entity => new CompanyInfoDto(entity))
                .ToListAsync(cancellationToken);

            return new Response(result);
        }
    }
}