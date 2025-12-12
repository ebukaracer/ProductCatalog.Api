using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Api.Middlewares;

/// <summary>
/// Handles exceptions globally and returns appropriate HTTP responses.
/// </summary>
public class ExceptionMiddleware(RequestDelegate next, IConfiguration config)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (BusinessException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 422;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;

            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            var showExDetails = config.GetValue<bool>("Exception:ShowDetails");

            var message = !isDevelopment ? "Something went wrong." :
                !showExDetails ? ex.Message : $"{ex.Message}\n{ex.StackTrace}";

            await context.Response.WriteAsJsonAsync(new { error = message });
        }
    }
}

public class BusinessException(string message) : Exception(message);

public class NotFoundException(string message) : Exception(message);

public class ConcurrencyException(string message) : Exception(message);