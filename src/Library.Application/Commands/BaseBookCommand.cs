using Library.Application.DTOs;
using MediatR;
using Newtonsoft.Json;

namespace Library.Application.Commands
{
    public abstract class BaseBookCommand : IRequest<BookDTO>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
    }
}