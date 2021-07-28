namespace PersonsHandBook.Domain.Models.Input
{
    public class SearchParams : PagedInput
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }

    }
}
