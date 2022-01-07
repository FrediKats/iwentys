using System.Linq;

namespace Iwentys.Infrastructure.DataAccess
{
    public interface ISpecification<T, TResult>
    {
        IQueryable<TResult> Specify(IQueryable<T> queryable);
    }
}
