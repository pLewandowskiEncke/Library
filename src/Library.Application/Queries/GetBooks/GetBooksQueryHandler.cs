using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Interfaces;
using MediatR;

namespace Library.Application.Queries.GetBooks
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDTO>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.BookRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }
    }
}
