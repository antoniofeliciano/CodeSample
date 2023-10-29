using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.DTOs.Assessment;
using Core.Enumns;
using Core.Interfaces.Services.Assessment;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Assessment
{
    public static class AssessmentQueryEndpoints
    {
        public static string InterfaceName => "AssessmentQuery";
        public static WebApplication AddAssessmentQueryEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapAssessmentQueryEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapAssessmentQueryEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapPost("/", async ([FromServices] IAssessmentQueryService assessmentQueryService,
                [FromBody] AssessmentQueryDto assessmentQueryDto) => await assessmentQueryService.AddAssessmentQueryAsync(assessmentQueryDto))
                .WithName("AddAssessmentQuery").RequirePermission(InterfaceName, PermissionType.create);

            mapper.WithTags(InterfaceName)
                .MapGet("/AssessmentQueriesForGrid", async ([FromServices] IAssessmentQueryService assessmentQueryService,
                [FromQuery] string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction) => await assessmentQueryService.GetAssessmentQueryForGridAsync(searchTerm, pageNumber, pageSize, orderBy, direction))
                .WithName("AssessmentQueriesForGrid").RequirePermission(InterfaceName, PermissionType.read);

            mapper.WithTags(InterfaceName)
                .MapPut("/", async ([FromServices] IAssessmentQueryService assessmentQueryService,
                [FromBody] AssessmentQueryDto assessmentQueryDto) => await assessmentQueryService.EditAssessmentQueryAsync(assessmentQueryDto))
                .WithName("EditAssessmentQuery").RequirePermission(InterfaceName, PermissionType.update);

            mapper.WithTags(InterfaceName)
                .MapDelete("/", async ([FromServices] IAssessmentQueryService assessmentQueryService,
                [FromQuery] Guid assessmentQueryId) => await assessmentQueryService.DeleteAssessmentQueryAsync(assessmentQueryId))
                .WithName("DeleteAssessmentQuery").RequirePermission(InterfaceName, PermissionType.delete);


            mapper.WithTags(InterfaceName)
                .MapGet("/QueryFile", async ([FromServices] IAssessmentQueryService assessmentQueryService) => await assessmentQueryService.GetAssessmentQueryFileAsync())
                .WithName("QueryFile").RequirePermission(InterfaceName, PermissionType.read);
            
            mapper.WithTags(InterfaceName)
                .MapGet("/DownloadQueryFile", async ([FromServices] IAssessmentQueryService assessmentQueryService) => await assessmentQueryService.GetAssessmentQueryFileAsync())
                .WithName("DownloadQueryFile").RequireApiKey();

            return mapper;
        }

    }
}
