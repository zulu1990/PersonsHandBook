using PersonsHandBook.Domain.Models.Enum;

namespace PersonsHandBook.Domain.Models.Entity
{
    public class Contact : BaseEntity
    {
        
        public Relation RelationType { get; set; }
        public Sex Sex { get; set; }

        //public Photo Photo { get; set; }
        //public int PhotoId { get; set; }
        
        public Person Person { get; set; }
        /// <summary>
        /// personId who owns contact
        /// </summary>
        public int PersonId { get; set; }

        public Photo Photo { get; set; }

        public string MobileNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string HomePhoneNumber { get; set; }

        public string Name { get; set; }

        public string City { get; set; }


    }
}
