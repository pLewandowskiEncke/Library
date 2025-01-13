using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;

namespace Library.Domain.BookStates
{
    public class OnTheShelfState : IBookState
    {
        public BookStatus Status => BookStatus.OnTheShelf;

        public IBookState Borrow(Book book)
        {
            return new BorrowedState();
        }

        public IBookState Return(Book book)
        {
            throw new InvalidBookStateException("Cannot return a book that is on the shelf.");
        }

        public IBookState MarkAsDamaged(Book book)
        {
            return new DamagedState();
        }

        public IBookState PlaceOnShelf(Book book)
        {
            return new OnTheShelfState();
        }
    }
}
