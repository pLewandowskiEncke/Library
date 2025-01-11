using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookDTO> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(request.Id);
            if (book == null)
            {
                throw new NotFoundException("Book not found");
            }
            switch (request.Status)
            {
                case BookStatus.OnTheShelf:
                    book.PlaceOnShelf();
                    break;
                case BookStatus.Borrowed:
                    book.Borrow();
                    break;
                case BookStatus.Returned:
                    book.Return();
                    break;
                case BookStatus.Damaged:
                    book.MarkAsDamaged();
                    break;
                default:
                    throw new InvalidBookStateException("Invalid state");
            }
            _mapper.Map(request, book);
            _unitOfWork.BeginTransaction();
            await _unitOfWork.BookRepository.UpdateAsync(book);
            _unitOfWork.Commit();
            return _mapper.Map<BookDTO>(book);
        }
    }
}