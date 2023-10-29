namespace Core.Results
{
    public class ErrorResult : Result
    {
        public override ResultType ResultType => ResultType.Error;
        public override IEnumerable<string>? Errors { get; protected set; } = new List<string>();
        public ErrorResult(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
                Errors = new[] { error };
        }
        public ErrorResult(IEnumerable<string> errors)
        {
            if (errors.Any())
                ((List<string>)Errors).AddRange(errors);
        }
    }
    public class ErrorResult<T> : Result<T>
    {
        public override ResultType ResultType => ResultType.Error;
        public override IEnumerable<string>? Errors { get; protected set; } = new List<string>();
        public ErrorResult(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
                Errors = new[] { error };
        }
        public ErrorResult(IEnumerable<string> errors)
        {
            if (errors.Any())
                ((List<string>)Errors).AddRange(errors);
        }
    }
}
