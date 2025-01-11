using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
