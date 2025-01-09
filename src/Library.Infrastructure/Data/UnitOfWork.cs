using Library.Domain.Interfaces;
using NHibernate;

namespace Library.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;
        private ITransaction _transaction;
        private readonly IBookRepository _bookRepository;

        public UnitOfWork(ISession session, IBookRepository bookRepository)
        {
            _session = session;
            _bookRepository = bookRepository;
        }

        public IBookRepository BookRepository => _bookRepository;

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                {
                    _transaction.Commit();
                }
            }
            catch
            {
                if (_transaction != null && _transaction.IsActive)
                {
                    _transaction.Rollback();
                }
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            try
            {
                if (_transaction != null && _transaction.IsActive)
                {
                    _transaction.Rollback();
                }
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _session.Dispose();
        }
    }
}
