namespace Library.Shared.Exceptions
{
    [Serializable]
    public class InvalidBookStateException : Exception
    {
        public InvalidBookStateException()
        {
        }

        public InvalidBookStateException(string? message) : base(message)
        {
        }

        public InvalidBookStateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}