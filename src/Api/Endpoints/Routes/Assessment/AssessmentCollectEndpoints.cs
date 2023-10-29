using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.DTOs.Assessment;
using Core.Enumns;
using Core.Interfaces.Services.Assessment;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Assessment
{
    public static class AssessmentCollectEndpoints
    {
        public static string InterfaceName => "AssessmentCollect";
        public static WebApplication AddAssessmentCollectEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapAssessmentCollectEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapAssessmentCollectEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapPost("/", async ([FromServices] IAssessmentCollectService assessmentCollectService,
                [FromBody] AssessmentCollectDto assessmentCollectDto) => await assessmentCollectService.AddAssessmentCollectAsync(assessmentCollectDto))
                .WithName("AddAssessmentCollect").RequirePermission(InterfaceName, PermissionType.create);

            mapper.WithTags(InterfaceName)
                .MapGet("/AssessmentCollectsForGrid", async ([FromServices] IAssessmentCollectService assessmentCollectService,
                [FromQuery] string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction) => await assessmentCollectService.GetAssessmentCollectForGridAsync(searchTerm, pageNumber, pageSize, orderBy, direction))
                .WithName("AssessmentCollectsForGrid").RequirePermission(InterfaceName, PermissionType.read);

            mapper.WithTags(InterfaceName)
                .MapPut("/", async ([FromServices] IAssessmentCollectService assessmentCollectService,
                [FromBody] AssessmentCollectDto assessmentCollectDto) => await assessmentCollectService.EditAssessmentCollectAsync(assessmentCollectDto))
                .WithName("EditAssessmentCollect").RequirePermission(InterfaceName, PermissionType.update);

            mapper.WithTags(InterfaceName)
                .MapDelete("/", async ([FromServices] IAssessmentCollectService assessmentCollectService,
                [FromQuery] Guid assessmentCollectId) => await assessmentCollectService.DeleteAssessmentCollectAsync(assessmentCollectId))
                .WithName("DeleteAssessmentCollect").RequirePermission(InterfaceName, PermissionType.delete);

            mapper.WithTags(InterfaceName)
               .MapPost("/SendCollect", async ([FromServices] IAssessmentCollectService assessmentCollectService,
                [FromBody] EncriptedCollectDto encriptedCollect) => await assessmentCollectService.AddEncriptedCollectAsync(encriptedCollect))
                .WithName("SendCollect").RequireApiKey();
            return mapper;
        }

    }
}
