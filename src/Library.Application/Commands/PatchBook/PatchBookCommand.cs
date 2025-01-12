using Library.Application.DTOs;
using Library.Domain.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Library.Application.Commands.PatchBook
{
    public class PatchBookCommand : BaseBookCommand, IRequest<BookDTO>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public BookStatus? Status { get; set; }
    }
}
