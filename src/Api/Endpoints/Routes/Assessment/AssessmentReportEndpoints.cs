using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.Enumns;
using Core.Interfaces.Services.Assessment;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Assessment
{
    public static class AssessmentReportEndpoints
    {
        public static string InterfaceName => "AssessmentReport";
        public static WebApplication AddAssessmentReportEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapAssessmentReportEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapAssessmentReportEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapGet("/CollectData", async ([FromServices] IAssessmentReportService assessmentReportService, [FromQuery] Guid collectId) => await assessmentReportService.GetAssessmentCollectDataAsync(collectId))
                .WithName("CollectData").RequirePermission(InterfaceName, PermissionType.read);

            return mapper;
        }

    }
}
