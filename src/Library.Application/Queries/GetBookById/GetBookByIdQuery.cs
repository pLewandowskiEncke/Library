using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<BookDTO>
    {
        public int Id { get; set; }

        public GetBookByIdQuery(int id)
        {
            Id = id;
        }
    }
}
