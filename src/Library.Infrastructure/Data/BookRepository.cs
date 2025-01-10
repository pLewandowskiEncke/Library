using Library.Domain.Entities;
using Library.Domain.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Infrastructure.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly ISession _session;

        public BookRepository(ISession session)
        {
            _session = session;
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _session.GetAsync<Book>(id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _session.QueryOver<Book>().ListAsync();
        }

        public async Task AddAsync(Book book)
        {
            await _session.SaveAsync(book);
        }

        public async Task UpdateAsync(Book book)
        {
            await _session.UpdateAsync(book);
        }

        public async Task DeleteAsync(Book book)
        {
            await _session.DeleteAsync(book);
        }

        public async Task<IEnumerable<Book>> GetBooks(int pageNumber, int pageSize, string sortBy, bool ascending)
        {
            var criteria = _session.CreateCriteria<Book>();

            // Apply sorting
            if (ascending)
            {
                criteria.AddOrder(Order.Asc(sortBy));
            }
            else
            {
                criteria.AddOrder(Order.Desc(sortBy));
            }

            // Apply paging
            criteria.SetFirstResult((pageNumber - 1) * pageSize);
            criteria.SetMaxResults(pageSize);

            return await criteria.ListAsync<Book>();
        }
    }
}
