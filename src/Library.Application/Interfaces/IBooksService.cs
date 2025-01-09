using Library.Application.Commands.CreateBook;
using Library.Application.DTOs;

namespace Library.Application.Interfaces
{
    public interface IBooksService
    {
        Task<BookDTO> CreateBookAsync(CreateBookCommand command);
        Task<BookDTO> UpdateBookAsync(UpdateBookCommand command);
        //Task DeleteBookAsync(DeleteBookCommand command);
        //Task<BookDTO> GetBookByIdAsync(GetBookByIdQuery query);
        //Task<PagedList<BookDTO>> GetBooksAsync(GetBooksQuery query);
    }
}