using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonsHandBook.Domain.Models.Enum;

namespace PersonsHandBook.Models
{
    public class UpdateContactModel
    {
        public int Id { get; set; }
        public int PersonId { get; set; }

        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public Relation RelationType { get; set; }
        public Sex Sex { get; set; }
    }
}
