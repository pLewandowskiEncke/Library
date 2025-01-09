using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDTO>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BookDTO> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransaction();
            var book = _mapper.Map<Book>(request);
            await _unitOfWork.BookRepository.AddAsync(book);
            _unitOfWork.Commit();
            return _mapper.Map<BookDTO>(book);
        }
    }
}