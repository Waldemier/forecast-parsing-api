using System.Linq;
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
        
        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}