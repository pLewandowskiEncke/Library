﻿using AutoMapper;
using Library.Application.Commands.CreateBook;
using Library.Application.Commands.PatchBook;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Mappings
{
    public class AutomapperBookProfile : Profile
    {
        public AutomapperBookProfile()
        {
            CreateMap<CreateBookCommand, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<UpdateBookCommand, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<PatchBookCommand, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForAllMembers(opt => opt.PreCondition((src, dest, member) => member != null)); 

            CreateMap<Book, BookDTO>();
        }
    }
}
