using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Post.Api.Extensions;
using Post.Api.Filters;
using Post.Infrastructure;
using Post.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Log.Logger = new LoggerConfiguration()
//     .ReadFrom.Configuration(builder.Configuration)
//     .CreateLogger();
//
// builder.Host.UseSerilog();

builder
    .Services
    .AddIdentity()
    .AddAuthorization()
    .AddJwtBearerAuthentication(builder.Configuration)
    .AddFluentValidation()
    .AddMediator()
    .AddPipelineBehaviours()
    .AddAzureBlobStorage(builder.Configuration)
    .AddFluentEmail(builder.Configuration)
    .AddDatabaseContext(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddCors()
    .AddOpenApi()
    .AddControllers(options =>
    {
        options.Filters.Add<ResultActionFilter>();
        options.Filters.Add<ExceptionFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.UseCors(policyBuilder => 
    policyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Debug"))
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddPreferredSecuritySchemes(JwtBearerDefaults.AuthenticationScheme);
        options.AddHttpAuthentication(
            JwtBearerDefaults.AuthenticationScheme, 
            scheme => scheme.Token = "{your_token_here}");
    });
}

// app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

await dbContext.Database.MigrateAsync();
await seeder.SeedAsync();

await app.RunAsync();