using System.Linq.Expressions;

namespace asyncDrive.API.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category

        Task<IEnumerable<T>> GetAllAsync(string? filterOn, string? filterQuery, string? sortBy, bool isAscending = true, int pageNumber = 1, int pageSize = 1000, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);

        Task<T> AddAsync(T entity);

        Task<T> RemoveAsync(T entity);

        Task<IEnumerable<T>> RemoveRangeAsync(IEnumerable<T> entities);

    }
}
