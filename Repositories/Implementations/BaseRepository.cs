using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ForecastAPI.Data;
using ForecastAPI.Repositories.Interfaces;

namespace ForecastAPI.Repositories.Implementations
{
    public abstract class BaseRepository<T>: IBaseRepository<T> where T: class
    {
        private ApplicationDbContext _context;
        protected BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task CreateAsync(T entity) => 
            await _context.Set<T>().AddAsync(entity);

        public void Update(T entity) =>
            _context.Set<T>().Update(entity);

        public void Remove(T entity) =>
            _context.Set<T>().Remove(entity);

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> condition) =>
            _context.Set<T>().Where(condition);
        
        public bool CheckByCondition(Expression<Func<T, bool>> condition) =>
            _context.Set<T>().Any(condition);
        
        public IQueryable<T> GetAll() =>
            _context.Set<T>();

        public async Task<int> SaveChangesAsync() =>
             await _context.SaveChangesAsync();
    }
}