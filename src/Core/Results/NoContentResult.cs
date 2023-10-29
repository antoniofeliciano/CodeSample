using System.Text.Json.Serialization;

namespace Core.Results
{
    public class NoContentResult : Result
    {
        [JsonIgnore]
        public override ResultType ResultType => ResultType.NoContent;
        public override IEnumerable<string>? Errors { get; protected set; }
        public NoContentResult(IEnumerable<string> errors)
        {
            if (errors.Any())
                Errors = errors;
        }
    }
    public class NoContentResult<T> : Result<T>
    {
        [JsonIgnore]
        public override ResultType ResultType => ResultType.NoContent;
        public override IEnumerable<string>? Errors { get; protected set; }
        public NoContentResult(IEnumerable<string> errors)
        {
            if (errors.Any())
                Errors = errors;
        }
        public NoContentResult() { }
    }
}
