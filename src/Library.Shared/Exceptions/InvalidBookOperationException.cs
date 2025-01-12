namespace Library.Shared.Exceptions
{
    [Serializable]
    public class InvalidBookOperationException : Exception
    {
        public string Operation { get; set; } = "Operation not allowed.";

        public InvalidBookOperationException()
        {
        }

        public InvalidBookOperationException(string? message) : base(message)
        {
        }

        public InvalidBookOperationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}