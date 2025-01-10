using Library.Application.DTOs;
using Library.Domain.Enums;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<BookDTO>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
    }
}