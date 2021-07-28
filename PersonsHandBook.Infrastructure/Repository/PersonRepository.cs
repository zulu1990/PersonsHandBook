using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonsHandBook.Domain.Interfaces;
using PersonsHandBook.Domain.Models;
using PersonsHandBook.Domain.Models.Entity;
using PersonsHandBook.Domain.Models.Output;

namespace PersonsHandBook.Infrastructure.Repository
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(DbFactory factory, ILoggerFactory loggerFactory) : base(factory, loggerFactory)
        {

        }

        public async Task<Person> AddNewPerson(Person person)
        {
            return await Add(person);
        }

        public IQueryable<Person> GetPersonListBy(Expression<Func<Person, bool>> func)
        {
            return List(func);
        }


        public Result<Person> RemovePerson(int personId)
        {
            try
            {
                var list = GetPersonListBy(x => x.Id == personId).Include(x => x.Contacts).Include(p => p.Photo).ToList();

                var person = list.FirstOrDefault();

                if (person == null)
                    return new Result<Person> { Success = false, ErrorMessage = Constants.PersonNotFound };

                Delete(person);

                return new Result<Person> { Success = true, Data = person };

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"RemovePerson: {personId}");
                return new Result<Person> { Success = false, ErrorMessage = Constants.Exception };
            }

        }
    }
}
