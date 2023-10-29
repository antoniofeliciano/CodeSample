namespace Core.Exceptions
{
    public class InvalidCollectException : Exception
    {
        public InvalidCollectException(string message) : base(message) { }
        public InvalidCollectException() : base("Invalid collect.") { }
    }
}
