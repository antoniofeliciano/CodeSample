using Api.Endpoints.Filters;
using ApplicationServices.Assessment;
using ApplicationServices.Authentication;
using ApplicationServices.Permissions;
using ApplicationServices.Tenants;
using Core.DTOs.Authentication;
using Core.DTOs.Configurations;
using Core.Entities.Assessment;
using Core.Entities.Authentication;
using Core.Entities.Permissions;
using Core.Entities.Tenants;
using Core.Interfaces.Repositories.Bases;
using Core.Interfaces.Services.Assessment;
using Core.Interfaces.Services.Authentication;
using Core.Interfaces.Services.Permissions;
using Core.Interfaces.Services.Tenants;
using Core.Validators.Authentication;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Data.Repositories.Assessment;
using Infrastructure.Data.Repositories.Authentication;
using Infrastructure.Data.Repositories.Permissions;
using Infrastructure.Data.Repositories.Tenants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {

        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            TokenSettings tokenSettings = new();
            configuration.GetSection("TokenSettings").Bind(tokenSettings);
            services.AddSingleton(tokenSettings);
        }
        public static void AddApplicationCore(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            services.AddValidatorsFromAssemblyContaining<UserValidator>();
            services.AddAutoMapper(typeof(LoginValidator));
            services.AddScoped<IAuthorizationHandler, ApiKeyHandler>();



            #region Services

            #region System Internals
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<IInterfaceService, InterfaceService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISystemApiKeyService, SystemApiKeyService>();
            services.AddScoped<IUserService, UserService>();
            #endregion




            #region System Modules
            services.AddScoped<IAssessmentQueryService, AssessmentQueryService>();
            services.AddScoped<IAssessmentCollectService, AssessmentCollectService>();
            services.AddScoped<IAssessmentReportService, AssessmentReportService>();
            services.AddScoped<IDatabaseTypeService, DatabaseTypeService>();


            #endregion

            #endregion


            #region Repositories
            services.AddScoped<IBaseEntityRepository<Tenant>, TenantRepository>();
            services.AddScoped<IBaseTenantEntityRepository<User>, UserRepository>();
            services.AddScoped<IBaseTenantEntityRepository<Role>, RoleRepository>();
            services.AddScoped<IBaseTenantEntityRepository<SystemApiKey>, SystemApiKeyRepository>();
            services.AddScoped<IBaseTenantEntityRepository<Area>, AreaRepository>();
            services.AddScoped<IBaseTenantEntityRepository<Interface>, InterfaceRepository>();
            services.AddScoped<IBaseTenantEntityRepository<Permission>, PermissionRepository>();



            services.AddScoped<IBaseTenantEntityRepository<AssessmentQuery>, AssessmentQueryRepository>();
            services.AddScoped<IBaseTenantEntityRepository<AssessmentCollect>, AssessmentCollectRepository>();
            services.AddScoped<IBaseEntityRepository<DatabaseType>, DatabaseTypeRepository>();


            #endregion

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
        }
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            services.AddDbContext<DefaultContext>(options => options.UseSqlServer(configuration.GetConnectionString("Brazabot")));
        }

        public static void AddAuthenticationSettings(this IServiceCollection services)
        {
            var secret = Encoding.ASCII.GetBytes(AuthenticationSettings.AuthenticationSecret);
            services.AddAuthentication(c =>
            {
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(c =>
            {
                c.RequireHttpsMetadata = false;
                c.SaveToken = true;
                c.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "Netbrokers.Brazabot",
                    ValidAudience = "Netbrokers.Brazabot"
                };
            });

            services.AddAuthorization();


        }

        public static void AddSwagggerInfo(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Brazabot Monitoring - Api",
                    Contact = new OpenApiContact()
                    {
                        Name = "Netbrokers",
                        Email = "antonio.feliciano@netbrokers.com",
                        Url = new Uri("http://www.Netbrokers.com.br")
                    }
                });

                // Adicione o esquema de segurança JWT
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Token JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };
                o.AddSecurityDefinition("Bearer", securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        Array.Empty<string>()
                    }
                };
                o.AddSecurityRequirement(securityRequirement);
            });
        }
    }
}