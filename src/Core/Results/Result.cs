namespace Core.Results
{
    public abstract class Result
    {
        public virtual ResultType ResultType { get; }
        public virtual IEnumerable<string>? Errors { get; protected set; }
    }
    public abstract class Result<T> : Result
    {
        public virtual T? Data { get; protected set; }

    }
}
