using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using MediatR;

namespace Library.Application.Queries.GetBooks
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, BookListDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookListDTO> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.BookRepository.GetBooks(request.PageNumber, request.PageSize, request.SortBy, request.Ascending);
            return new BookListDTO
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SortBy = request.SortBy,
                Ascending = request.Ascending,
                Books = _mapper.Map<IEnumerable<BookDTO>>(books)
            };
        }
    }
}
