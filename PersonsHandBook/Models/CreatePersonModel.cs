using PersonsHandBook.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PersonsHandBook.Domain.Models.Enum;

namespace PersonsHandBook.Models
{
    public class CreatePersonModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MinLength(11, ErrorMessage = "Personal Number Must Be 11 Symbol")]
        [MaxLength(11, ErrorMessage = "Personal Number Must Be 11 Symbol")]
        public string PersonalNumber { get; set; }

        public Sex Sex { get; set; }

        [MinAge(18)]
        public DateTime DateOfBirth { get; set; }

    }
}
