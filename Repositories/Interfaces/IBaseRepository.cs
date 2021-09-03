using System.Linq;
using System.Threading.Tasks;

namespace ForecastAPI.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task CreateAsync(T entity);
        IQueryable<T> GetAll();
        Task<int> SaveChangesAsync();
    }
}