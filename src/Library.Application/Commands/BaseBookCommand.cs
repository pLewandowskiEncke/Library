using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Commands
{
    public abstract class BaseBookCommand : IRequest<BookDTO>
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
    }
}