namespace PersonsHandBook.Domain.Models.Input
{
    public class PagedInput
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
