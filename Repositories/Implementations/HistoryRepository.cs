using ForecastAPI.Data;
using ForecastAPI.Data.Entities;
using ForecastAPI.Repositories.Interfaces;

namespace ForecastAPI.Repositories.Implementations
{
    public class HistoryRepository: BaseRepository<History>, IHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        public HistoryRepository(ApplicationDbContext context) : 
            base(context)
        {
            _context = context;
        }
    }
}