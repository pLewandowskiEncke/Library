using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;

namespace Library.Domain.BookStates
{
    public class DamagedState : IBookState
    {
        public BookStatus Status => BookStatus.Damaged;

        public IBookState Borrow(Book book)
        {
            throw new InvalidBookStateException("Cannot borrow a damaged book.");
        }

        public IBookState Return(Book book)
        {
            throw new InvalidBookStateException("Cannot return a damaged book.");
        }

        public IBookState MarkAsDamaged(Book book)
        {
            throw new InvalidBookStateException("The book is already marked as damaged.");
        }

        public IBookState PlaceOnShelf(Book book)
        {
            return new OnTheShelfState();
        }
    }
}
