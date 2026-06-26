using FluentValidation;

namespace StackOverflowLite.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            var errors = ex.Errors.Select(e => new 
            { 
                field = e.PropertyName, 
                message = e.ErrorMessage 
            });
            await context.Response.WriteAsJsonAsync(new { errors });
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "Internal server error", detail = ex.Message });
        }
    }
}
