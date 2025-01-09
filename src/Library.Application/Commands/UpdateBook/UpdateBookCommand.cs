using Library.Application.DTOs;
using Library.Domain.Enums;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class UpdateBookCommand : IRequest<BookDTO>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public BookStatus Status { get; set; }
    }
}