using Library.Application.DTOs;
using Library.Domain.Enums;
using MediatR;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Library.Application.Commands.CreateBook
{
    public class UpdateBookCommand : IRequest<BookDTO>
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public BookStatus Status { get; set; }
    }
}