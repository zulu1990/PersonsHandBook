using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PersonsHandBook.Domain.Interfaces;
using PersonsHandBook.Domain.Models;
using PersonsHandBook.Domain.Models.Entity;

namespace PersonsHandBook.Infrastructure.Repository
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(DbFactory factory, ILoggerFactory loggerFactory) : base(factory, loggerFactory)
        {

        }

        public async Task AddContact(Contact newContact)
        {
            await Add(newContact);
        }

        public async Task<Contact> GetContactById(int contactId)
        {
            return await GetById(contactId);
        }

        public IQueryable<Contact> GetContactListBy(Expression<Func<Contact, bool>> func)
        {
            return List(func);
        }

        public void UpdateContact(Contact contact)
        {
            Update(contact);
        }
    }
}
