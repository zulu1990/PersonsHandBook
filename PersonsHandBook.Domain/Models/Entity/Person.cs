using System;
using System.Collections.Generic;
using PersonsHandBook.Domain.Models.Enum;

namespace PersonsHandBook.Domain.Models.Entity
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }
        public Sex Sex { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Photo Photo { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
