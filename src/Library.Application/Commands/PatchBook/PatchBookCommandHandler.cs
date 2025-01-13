using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using MediatR;

namespace Library.Application.Commands.PatchBook
{
    public class PatchBookCommandHandler : IRequestHandler<PatchBookCommand, BookDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatchBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookDTO> Handle(PatchBookCommand request, CancellationToken cancellationToken)
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
