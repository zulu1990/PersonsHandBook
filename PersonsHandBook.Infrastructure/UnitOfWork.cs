using PersonsHandBook.Domain.Interfaces;
using System.Threading.Tasks;

namespace PersonsHandBook.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbFactory _dbFactory;

        public UnitOfWork(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<bool> CommitAsync()
        {
            var result = await _dbFactory.DbContext.SaveChangesAsync() > 0;

            return result;
        }
    }
}
