using Api.Configurations;
using Api.Endpoints.Extensions.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagggerInfo();
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddApplicationCore();

builder.Services.AddAuthenticationSettings();



//var dbcontext = builder.Services.BuildServiceProvider().GetRequiredService<DefaultContext>();
//dbcontext.Database.Migrate();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x.AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials

app.UseHttpsRedirection();

app.AddSystemEndpoint();

app.UseAuthentication();
app.UseAuthorization();


app.Run();