using PersonsHandBook.Domain.Models.Entity;
using PersonsHandBook.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonsHandBook.Domain.Models.Input
{
    public class DetailSearchPerson : PagedInput
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }
        public Sex? Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
