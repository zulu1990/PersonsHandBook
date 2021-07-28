using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PersonsHandBook.Domain.Models.Entity;
using PersonsHandBook.Domain.Models.Output;

namespace PersonsHandBook.Domain.Interfaces
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<Person> AddNewPerson(Person person);
        IQueryable<Person> GetPersonListBy(Expression<Func<Person, bool>> func);
        Result<Person> RemovePerson(int personId);
    }
}
