using Library.Domain.Enums;

namespace Library.Domain.Entities
{
    public class Book
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Author { get; set; }
        public virtual string ISBN { get; set; }
        public virtual BookStatus Status { get; set; }
    }
}