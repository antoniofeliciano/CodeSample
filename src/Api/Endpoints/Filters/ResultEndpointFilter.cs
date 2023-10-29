using Core.Results;
using System.Reflection;

namespace Api.Endpoints.Filters
{
    public class ResultEndpointFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var result = await next(context) as Result;

            return result?.ResultType switch
            {
                ResultType.Ok => result.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList().Exists(p => p.Name == "Data") ? Results.Ok(result) : Results.NoContent(),
                ResultType.Accepted => Results.Accepted(),
                ResultType.Created => Results.StatusCode(StatusCodes.Status201Created),
                ResultType.NotFound => Results.NotFound(result),
                ResultType.Unauthorized => Results.Unauthorized(),
                ResultType.Invalid => Results.BadRequest(result),
                ResultType.Error => Results.BadRequest(result),
                ResultType.Unexpected => Results.BadRequest(result),
                ResultType.NoContent => Results.NoContent(),
                _ => throw new Exception("An unhandled result has occurred as a result of a service call."),
            };
        }
    }
}
