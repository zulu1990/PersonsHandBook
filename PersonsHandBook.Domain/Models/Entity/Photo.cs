using System.ComponentModel.DataAnnotations.Schema;

namespace PersonsHandBook.Domain.Models.Entity
{
    public class Photo : BaseEntity
    {
        public string Url { get; set; }
        
        [ForeignKey("Person")]
        public int? PersonId { get; set; }
        public Person Person { get; set; }

        public int? ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
