using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PersonsHandBook.Domain.Models.Entity;

namespace PersonsHandBook.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        IQueryable<T> List(Expression<Func<T, bool>> expression);
        Task<T> GetById(int id);
    }
}
