namespace Core.Exceptions
{
    public class InvalidTenantException : Exception
    {
        public InvalidTenantException(string message) : base(message) { }
        public InvalidTenantException() : base("Invalid tenant.") { }
    }
}
