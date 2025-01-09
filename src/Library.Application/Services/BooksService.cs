using AutoMapper;
using Library.Application.Commands.CreateBook;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;

namespace Library.Application.Services
{
    public class BooksService : IBooksService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BooksService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookDTO> CreateBookAsync(CreateBookCommand command)
        {
            var book = _mapper.Map<Book>(command);
            await _unitOfWork.BookRepository.AddAsync(book);
            _unitOfWork.Commit();
            return _mapper.Map<BookDTO>(book);
        }

        public async Task<BookDTO> UpdateBookAsync(UpdateBookCommand command)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(command.Id);
            if (book == null)
            {
                throw new NotFoundException("Book not found");
            }

            _mapper.Map(command, book);
            _unitOfWork.Commit();
            return _mapper.Map<BookDTO>(book);
        }

        //public async Task DeleteBookAsync(DeleteBookCommand command)
        //{
        //    var book = await _unitOfWork.BookRepository.GetByIdAsync(command.Id);
        //    if (book == null)
        //    {
        //        throw new NotFoundException("Book not found");
        //    }

        //    await _unitOfWork.BookRepository.DeleteAsync(book);
        //    _unitOfWork.Commit();
        //}

        //public async Task<BookDTO> GetBookByIdAsync(GetBookByIdQuery query)
        //{
        //    var book = await _unitOfWork.BookRepository.GetByIdAsync(query.Id);
        //    if (book == null)
        //    {
        //        throw new NotFoundException("Book not found");
        //    }

        //    return _mapper.Map<BookDTO>(book);
        //}

        //public async Task<PagedList<BookDTO>> GetBooksAsync(GetBooksQuery query)
        //{
        //    var books = await _unitOfWork.BookRepository.GetAllAsync();
        //    return _mapper.Map<PagedList<BookDTO>>(books);
        //}
    }
}