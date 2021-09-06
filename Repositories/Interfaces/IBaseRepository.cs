using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ForecastAPI.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> condition);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        bool CheckByCondition(Expression<Func<T, bool>> condition);
        Task<int> SaveChangesAsync();
    }
}