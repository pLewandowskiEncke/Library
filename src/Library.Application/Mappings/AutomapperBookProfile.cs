using AutoMapper;
using Library.Application.Commands.CreateBook;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Mappings
{
    public class AutomapperBookProfile : Profile
    {
        public AutomapperBookProfile()
        {          
            CreateMap<CreateBookCommand, Book>();

            CreateMap<Book, BookDTO>();
        }
    }
}
