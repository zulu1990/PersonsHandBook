using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonsHandBook.Domain.Models.Output
{
    public class PersonsContactReport
    {
        public int Colleges { get; set; }
        public int Friends { get; set; }
        public int Relatives { get; set; }
        public int Ect { get; set; }
    }
}
