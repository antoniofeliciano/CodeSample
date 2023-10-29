namespace Core.Results
{
    public class InvalidResult : Result
    {
        public override ResultType ResultType => ResultType.Invalid;
        public override IEnumerable<string>? Errors { get; protected set; } = new List<string>();
        public InvalidResult(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
                Errors = new[] { error };
        }
        public InvalidResult(IEnumerable<string> errors)
        {
            if (errors.Any())
                Errors = errors;
        }
    }

    public class InvalidResult<T> : Result<T>
    {
        public override ResultType ResultType => ResultType.Invalid;
        public override IEnumerable<string>? Errors { get; protected set; }
        public override T? Data { get; protected set; }

        public InvalidResult(string error, T? data)
        {
            if (!string.IsNullOrWhiteSpace(error))
                Errors = new[] { error };

            if (data is not null)
                Data = data;
        }
        public InvalidResult(IEnumerable<string> errors, T? data)
        {
            if (errors.Any())
                ((List<string>)Errors!).AddRange(errors);
            if (data is not null)
                Data = data;
        }
    }
}
