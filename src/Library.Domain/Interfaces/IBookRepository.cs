using Library.Domain.Entities;

namespace Library.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(Book book);
        Task<IEnumerable<Book>> GetBooks(int pageNumber, int pageSize, string sortBy, bool ascending);
        Task<bool> IsISBNUniqueAsync(string ISBN);
    }
}
