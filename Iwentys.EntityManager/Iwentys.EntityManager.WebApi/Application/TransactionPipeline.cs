using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class TransactionPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly DbContext _context;

    public TransactionPipeline(DbContext context)
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