using AutoMapper;
using Library.Application.Commands.UpdateBook;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;
using MediatR;
using System.ComponentModel.DataAnnotations;

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
            _mapper.Map(request, book);
            if (request.Status.HasValue)
            {
                book.TryChangeStatus(request.Status.Value);
            }
            _unitOfWork.BeginTransaction();
            await _unitOfWork.BookRepository.UpdateAsync(book);
            _unitOfWork.Commit();
            return _mapper.Map<BookDTO>(book);
        }
    }
}