namespace Iwentys.EntityManager.DataAccess;

public interface ISpecification<T, TResult>
{
    IQueryable<TResult> Specify(IQueryable<T> queryable);
}