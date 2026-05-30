using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace GeneticAlgorithm.Api;

internal static class ApiJsonErrors
{
    public static void UseFriendlyJsonErrors(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (BadHttpRequestException ex) when (ex.InnerException is JsonException)
            {
                await WriteInvalidJsonResponse(context, ex.InnerException.Message);
            }
        });

        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                IExceptionHandlerFeature feature = context.Features.Get<IExceptionHandlerFeature>();
                Exception error = feature?.Error;
                if (error is BadHttpRequestException badRequest && badRequest.InnerException is JsonException)
                {
                    await WriteInvalidJsonResponse(context, badRequest.InnerException.Message);
                    return;
                }

                if (error != null && !context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new { error = error.Message });
                }
            });
        });
    }

    private static Task WriteInvalidJsonResponse(HttpContext context, string detail)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return context.Response.WriteAsJsonAsync(new
        {
            error = "Invalid JSON request body. Property names and string values must use double quotes.",
            hint = "Copy a valid body from GET /api/sample/optimization-request or GET /api/sample/simulation-request.",
            detail,
            invalidExample = "{maxTrucks: 6}",
            validExample = "{\"maxTrucks\": 6, \"maxLoaders\": 2, \"maxScalers\": 2, \"generations\": 20, \"simulation\": {\"coalVolume\": 10000}}"
        });
    }
}
