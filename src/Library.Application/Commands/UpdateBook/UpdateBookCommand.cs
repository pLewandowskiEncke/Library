using Library.Application.DTOs;
using Library.Domain.Enums;
using MediatR;

namespace Library.Application.Commands.UpdateBook
{
    public class UpdateBookCommand : BaseBookCommand, IRequest<BookDTO>
    {
        public BookStatus? Status { get; set; }
    }
}