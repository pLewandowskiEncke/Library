using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDTO>
    {
        private readonly IBooksService _booksService;
        private readonly IMapper _mapper;

        public CreateBookCommandHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public CreateBookCommandHandler(IBooksService booksService)
        {
            _booksService = booksService;
        }

        public async Task<BookDTO> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            return await _booksService.CreateBookAsync(request);
        }
    }
}