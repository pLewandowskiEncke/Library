using Library.Domain.BookStates;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;

namespace Library.Domain.Entities
{
    public partial class Book
    {
        public Book()
        {
            this.Status = BookStatus.OnTheShelf; // Default state
        }

        public virtual void Borrow()
        {
            var nextState = GetStateInstance().Borrow(this);
            this.Status = nextState.Status;
        }

        public virtual void Return()
        {
            var nextState = GetStateInstance().Return(this);
            this.Status = nextState.Status;
        }

        public virtual void MarkAsDamaged()
        {
            var nextState = GetStateInstance().MarkAsDamaged(this);
            this.Status = nextState.Status;
        }

        public virtual void PlaceOnShelf()
        {
            var nextState = GetStateInstance().PlaceOnShelf(this);
            this.Status = nextState.Status;
        }

        public virtual void TryChangeStatus(BookStatus status)
        {
            switch (status)
            {
                case BookStatus.OnTheShelf:
                    this.PlaceOnShelf();
                    break;
                case BookStatus.Borrowed:
                    this.Borrow();
                    break;
                case BookStatus.Returned:
                    this.Return();
                    break;
                case BookStatus.Damaged:
                    this.MarkAsDamaged();
                    break;
                default:
                    throw new InvalidBookStateException("Invalid state");
            }
        }

        private IBookState GetStateInstance()
        {
            return Status switch
            {
                BookStatus.OnTheShelf => new OnTheShelfState(),
                BookStatus.Borrowed => new BorrowedState(),
                BookStatus.Returned => new ReturnedState(),
                BookStatus.Damaged => new DamagedState(),
                _ => throw new InvalidOperationException("Invalid state")
            };
        }
    }
}
