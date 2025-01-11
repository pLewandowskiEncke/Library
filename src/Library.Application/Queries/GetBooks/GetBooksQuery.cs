using Library.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Library.Application.Queries.GetBooks
{
    public class GetBooksQuery : IRequest<BookListDTO>
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 100;

        public string SortBy { get; set; } = "Id";
        public bool Ascending { get; set; } = true;
    }
}
