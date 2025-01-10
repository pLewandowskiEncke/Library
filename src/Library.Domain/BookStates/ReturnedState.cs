using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;

namespace Library.Domain.BookStates
{
    public class ReturnedState : IBookState
    {
        public BookStatus Status => BookStatus.Returned;

        public IBookState Borrow(Book book)
        {
            throw new InvalidBookStateException("Cannot borrow a returned book.");
        }

        public IBookState Return(Book book)
        {
            throw new InvalidBookStateException("The book is already returned.");
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
