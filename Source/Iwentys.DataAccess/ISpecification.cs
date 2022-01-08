using System.Linq;

namespace Iwentys.DataAccess;

public interface ISpecification<T, TResult>
{
    IQueryable<TResult> Specify(IQueryable<T> queryable);
}