using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonsHandBook.Domain.Models.Output
{
    public class Result<T>
    {
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
    }

    public class Result
    {
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
    }
}