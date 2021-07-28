using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonsHandBook.Domain.Interfaces;
using PersonsHandBook.Domain.Models.Entity;

namespace PersonsHandBook.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public readonly ILogger _logger;
        private readonly DbFactory _dbFactory;
        private DbSet<T> _dbSet;

        public DbSet<T> DbSet => _dbSet ??= _dbFactory.DbContext.Set<T>();

        protected Repository(DbFactory dbFactory, ILoggerFactory loggerFactory)
        {
            _dbFactory = dbFactory;
            _logger = loggerFactory.CreateLogger<Repository<T>>();
        }

        public async Task<T> Add(T entity)
        {
            var result = await DbSet.AddAsync(entity);

            return result.Entity;
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public IQueryable<T> List(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression);
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                return await DbSet.SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"GetById in Repository {typeof(T)}");
                return null;
            }
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }


    }
}
