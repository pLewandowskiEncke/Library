using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Queries.GetBooks
{
    public class GetBooksQuery : IRequest<BookListDTO>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string SortBy { get; set; } = "Id";
        public bool Ascending { get; set; } = true;
    }
}
