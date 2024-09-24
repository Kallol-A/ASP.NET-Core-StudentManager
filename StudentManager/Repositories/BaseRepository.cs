using System.Threading.Tasks;

using StudentManager.Data;

namespace StudentManager.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _dbContext;

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
