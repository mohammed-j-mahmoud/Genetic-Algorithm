using System;
using System.Linq;
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
                    string message = GetFriendlyErrorMessage(error);
                    if (IsClientInputError(error))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(new { error = message });
                        return;
                    }

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new { error = message });
                }
            });
        });
    }

    private static bool IsClientInputError(Exception error)
    {
        if (error is ArgumentException)
            return true;

        if (error is AggregateException aggregate)
            return aggregate.Flatten().InnerExceptions.All(e => e is ArgumentException);

        return false;
    }

    private static string GetFriendlyErrorMessage(Exception error)
    {
        if (error is AggregateException aggregate)
        {
            var inner = aggregate.Flatten().InnerExceptions;
            if (inner.Count == 0)
                return error.Message;

            if (inner.All(e => e.Message == inner[0].Message))
                return inner[0].Message;

            return inner[0].Message;
        }

        return error.Message;
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
            validExample = "{\"maxTrucks\": 6, \"maxLoaders\": 2, \"maxScalers\": 2, \"generations\": 20, \"simulation\": {\"coalVolume\": 10000, \"truckLoadVolume\": 20, \"truckCount\": 6, \"loaderCount\": 2, \"scalerCount\": 2}}"
        });
    }
}
