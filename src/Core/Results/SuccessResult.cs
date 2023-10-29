namespace Core.Results
{
    public class AcceptedResult : Result
    {
        public override ResultType ResultType => ResultType.Ok;
        public AcceptedResult() { }
    }

    public class SuccessResult<T> : Result<T>
    {
        public override ResultType ResultType => ResultType.Ok;
        public override T? Data { get; protected set; }
        public SuccessResult() { }
        public SuccessResult(T? data) : this()
        {
            Data = data;
        }
    }
}
