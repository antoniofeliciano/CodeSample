using System.Text.Json.Serialization;

namespace Core.Results
{
    public class NotFoundResult : Result
    {
        [JsonIgnore]
        public override ResultType ResultType => ResultType.NotFound;
        public override IEnumerable<string>? Errors { get; protected set; }
        public NotFoundResult(IEnumerable<string> errors)
        {
            if (errors.Any())
                Errors = errors;
        }
    }
    public class NotFoundResult<T> : Result<T>
    {
        [JsonIgnore]
        public override ResultType ResultType => ResultType.NotFound;
        public override IEnumerable<string>? Errors { get; protected set; }
        public NotFoundResult(IEnumerable<string> errors)
        {
            if (errors.Any())
                Errors = errors;
        }
        public NotFoundResult() { }
    }
}
