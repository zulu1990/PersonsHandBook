using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PersonsHandBook.Domain.Models.Enum;
using PersonsHandBook.Services;

namespace PersonsHandBook.Models
{
    public class AddContactModel
    {
        public Relation RelationType { get; set; }
        public Sex Sex { get; set; }

        /// <summary>
        /// personId who owns contact
        /// </summary>
        public int PersonId { get; set; }

        [MinLength(4)]
        [MaxLength(50)]
        public string MobileNumber { get; set; }

        [MinLength(4)]
        [MaxLength(50)]
        public string OfficeNumber { get; set; }

        [MinLength(4)]
        [MaxLength(50)]
        public string HomePhoneNumber { get; set; }

        [NameValidation(5,50, ErrorMessageResourceType = (typeof(Resources.Locallizer.SharedResources)))]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }
    }
}
