using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookCommand : BaseBookCommand, IRequest<BookDTO>
    {
    }
}