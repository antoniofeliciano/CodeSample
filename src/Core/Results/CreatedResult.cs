namespace Core.Results
{
    public class CreatedResult : Result
    {
        public override ResultType ResultType => ResultType.Created;

        public override IEnumerable<string>? Errors { get; protected set; }
    }
}
