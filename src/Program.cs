using ProductCatalog.Api.Data;
using ProductCatalog.Api.Extensions;
using ProductCatalog.Api.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRepositories(builder);
builder.Services.AddHealthChecks();

var app = builder.Build();

// Redirect base URL to Scalar UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar");
    return Task.CompletedTask;
});

app.MapOpenApi();
app.MapScalarApiReference(options => { options.Title = "Product & Order API"; });

using var scope = app.Services.CreateScope();
await DbInitializer.InitializeAsync(scope.ServiceProvider, builder.Configuration);

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();