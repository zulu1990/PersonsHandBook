using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PersonsHandBook.Domain.Models;
using PersonsHandBook.Domain.Models.Entity;

namespace PersonsHandBook.Domain.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        IQueryable<Contact> GetContactListBy(Expression<Func<Contact, bool>> func);
        Task AddContact(Contact newContact);
        void UpdateContact(Contact contact);
        Task<Contact> GetContactById(int contactId);
    }
}
