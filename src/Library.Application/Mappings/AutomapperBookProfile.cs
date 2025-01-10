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
            CreateMap<CreateBookCommand, Book>()
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<Book, BookDTO>();
        }
    }
}
