using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using asyncDrive.API.Repositories.IRepository;
using asyncDrive.DataAccess;

namespace asyncDrive.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly asyncDriveDbContext dbContext;
        internal DbSet<T> dbSet;
        public Repository(asyncDriveDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
            //_dbContext.Users == dbSet
        }
        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(string? filterOn, string? filterQuery, string? sortBy, bool isAscending = true, int pageNumber = 1, int pageSize = 1000, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            //we can pass multiple navigation properties using , split ("Category,Address etc")
            if (!string.IsNullOrEmpty(includeProperties))//to implement eager loading for navigation properties
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> RemoveAsync(T entity)
        {
            await Task.Run(() =>
            {
                dbSet.Remove(entity);
            });
            return entity;
        }

        public async Task<IEnumerable<T>> RemoveRangeAsync(IEnumerable<T> entities)
        {
            await Task.Run(() =>
            {
                dbSet.RemoveRange(entities);
            });
            return entities;
        }

    }
}
