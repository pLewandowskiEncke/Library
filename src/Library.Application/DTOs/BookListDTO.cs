namespace Library.Application.DTOs
{
    public class BookListDTO
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public bool Ascending { get; set; }
        public IEnumerable<BookDTO> Books { get; set; }
    }
}
