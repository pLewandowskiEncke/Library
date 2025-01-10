using Library.Domain.Entities;
using Library.Domain.Enums;

namespace Library.Domain.Interfaces
{
    public interface IBookState
    {
        BookStatus Status { get; }
        IBookState Borrow(Book book);
        IBookState Return(Book book);
        IBookState MarkAsDamaged(Book book);
        IBookState PlaceOnShelf(Book book);
    }
}
