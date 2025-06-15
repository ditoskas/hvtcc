using Hvt.Infrastructure.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hvt.Infrastructure.Repositories.Models
{
    public abstract class RepositoryBase<T, TC>(TC repositoryContext) : IRepositoryBase<T>
        where T : class
        where TC : Microsoft.EntityFrameworkCore.DbContext
    {
        protected TC RepositoryContext = repositoryContext;

        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            if (trackChanges)
            {
                return RepositoryContext.Set<T>();
            }
            else
            {
                return RepositoryContext.Set<T>().AsNoTracking();
            }
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            if (trackChanges)
            {
                return RepositoryContext.Set<T>().Where(expression);
            }
            else
            {
                return RepositoryContext.Set<T>().Where(expression).AsNoTracking();
            }
        }
    }
}
