using AutoMapper;
using Library.Application.Commands.PatchBook;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;
using MediatR;

namespace Library.Application.Commands.UpdateBook
{
    public class PatchBookCommandHandler : IRequestHandler<PatchBookCommand, BookDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PatchBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookDTO> Handle(PatchBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(request.Id);
            if (book == null)
            {
                throw new NotFoundException("Book not found");
            }

            //_mapper.Map(request, book);

            if (!string.IsNullOrEmpty(request.Title))
            {
                book.Title = request.Title;
            }

            if (!string.IsNullOrEmpty(request.Author))
            {
                book.Author = request.Author;
            }

            if (!string.IsNullOrEmpty(request.ISBN))
            {
                book.ISBN = request.ISBN;
            }

            _unitOfWork.BeginTransaction();
            await _unitOfWork.BookRepository.UpdateAsync(book);
            _unitOfWork.Commit();
            return _mapper.Map<BookDTO>(book);
        }
    }
}
