using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonsHandBook.Domain.Models.Input
{
    public class GetContacts : PagedInput
    {
        public int PersonId { get; set; }
    }
}
