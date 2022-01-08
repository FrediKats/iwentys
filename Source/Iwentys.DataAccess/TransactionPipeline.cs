using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Iwentys.DataAccess
{
    public class TransactionPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IwentysDbContext _context;

        public TransactionPipeline(IwentysDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //TODO: transaction is not supported
            //await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);

            TResponse response = await next();

            await _context.SaveChangesAsync(cancellationToken);
            // await tx.CommitAsync(cancellationToken);

            return response;
        }
    }
}