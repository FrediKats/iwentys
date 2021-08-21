using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess
{
    public class TransactionPipeline<TDbContext, TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        public TransactionPipeline(TDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //TODO: transaction is not supported
            //await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);

            TResponse response = await next();

            await _context.SaveChangesAsync(cancellationToken);
            //await tx.CommitAsync(cancellationToken);

            return response;
        }
    }
}