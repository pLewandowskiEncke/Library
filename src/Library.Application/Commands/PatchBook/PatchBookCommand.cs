using Library.Application.DTOs;
using Library.Domain.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Library.Application.Commands.PatchBook
{
    public class PatchBookCommand : IRequest<BookDTO>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public BookStatus? Status { get; set; }
    }
}
