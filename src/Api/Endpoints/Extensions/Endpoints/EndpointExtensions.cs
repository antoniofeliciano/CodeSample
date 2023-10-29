using Api.Endpoints.Routes.Assessment;
using Api.Endpoints.Routes.Authentication;
using Api.Endpoints.Routes.Permissioning;

namespace Api.Endpoints.Extensions.Endpoints
{
    public static class EndpointExtensions
    {
        public static void AddSystemEndpoint(this WebApplication app)
        {
            app.AddAuthenticationEndpoints();
            app.AddTenantsEndpoints();
            app.AddAreaEndpoints();
            app.AddInterfaceEndpoints();
            app.AddRoleEndpoints();
            app.AddUserEndpoints();
            app.AddSystemApiKeyEndpoints();
            app.AddPermissionEndpoints();


            app.AddBusinessEndpoint();
        }

        private static void AddBusinessEndpoint(this WebApplication app)
        {
            app.AddAssessmentQueryEndpoints();
            app.AddAssessmentCollectEndpoints();
            app.AddAssessmentReportEndpoints();
            app.AddDatabaseTypeEndpoints();

        }
    }
}
