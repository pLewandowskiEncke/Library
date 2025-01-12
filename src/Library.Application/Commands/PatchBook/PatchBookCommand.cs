using Library.Application.DTOs;
using Library.Domain.Enums;
using MediatR;

namespace Library.Application.Commands.PatchBook
{
    public class PatchBookCommand : BaseBookCommand, IRequest<BookDTO>
    {
        public BookStatus? Status { get; set; }
    }
}
