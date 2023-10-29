namespace Core.Results
{
    public class UnexpectedResult : Result
    {
        public override ResultType ResultType => ResultType.Unexpected;
        public override IEnumerable<string>? Errors { get; protected set; }
        public UnexpectedResult() { }

        public UnexpectedResult(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
                Errors = new[] { error };
        }
        public UnexpectedResult(IEnumerable<string> errors)
        {
            if (errors.Any())
                Errors = errors;
        }
    }
}