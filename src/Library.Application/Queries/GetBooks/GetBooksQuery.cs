using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Queries.GetBooks
{
    public class GetBooksQuery : IRequest<IEnumerable<BookDTO>>
    {
    }
}
