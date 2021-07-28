using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PersonsHandBook.Models
{
    public class UploadPhotoModel
    {
        public IFormFile Photo { get; set; }
        public int? PersonId { get; set; }
        public int? ContactId { get; set; }

    }
}
