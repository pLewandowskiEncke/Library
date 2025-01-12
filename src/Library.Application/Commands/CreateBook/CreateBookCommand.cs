using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<BookDTO>
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
    }
}