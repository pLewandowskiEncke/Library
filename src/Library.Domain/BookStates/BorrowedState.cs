using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;

namespace Library.Domain.BookStates
{
    public class BorrowedState : IBookState
    {
        public BookStatus Status => BookStatus.Borrowed;

        public IBookState Borrow(Book book)
        {
            throw new InvalidBookStateException("The book is already borrowed.");
        }

        public IBookState Return(Book book)
        {
            return new ReturnedState();
        }

        public IBookState MarkAsDamaged(Book book)
        {
            throw new InvalidBookStateException("Cannot mark as damaged a borrowed book.");
        }

        public IBookState PlaceOnShelf(Book book)
        {
            throw new InvalidBookStateException("Cannot place a borrowed book on the shelf.");
        }
    }
}
