using System;

namespace Library.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository BookRepository { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
