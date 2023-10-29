namespace Core.Results
{
    public class UnauthorizedResult : Result
    {
        public override ResultType ResultType => ResultType.Unauthorized;
        public override IEnumerable<string>? Errors { get; protected set; }

        public UnauthorizedResult(string? error)
        {
            if (!string.IsNullOrWhiteSpace(error))
                Errors = new[] { error };
        }
    }

}
