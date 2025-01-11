using Library.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<BookDTO>
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string ISBN { get; set; }
    }
}