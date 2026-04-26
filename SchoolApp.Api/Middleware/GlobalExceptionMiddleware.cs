using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Api.Middleware;

// Custom middleware sounds fancy, but its just a class.
// Middleware classes have a specific "shape"
// 1. A constructor that accepts a "RequestDelegate"
// 2. A public InvokeAsync(HttpContext context) method
// ASP.NET Core discovers our middleware when we call app.UseMiddleware<T>()
// in the app section below the builder area in Program.cs
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    // Our constructor
    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // InvokeAsync method
    public async Task InvokeAsync(HttpContext context)
    {
        // HttpContext - Holds EVERYTHING about a given HTTP Request AND its response
        try
        {
            // Pass the request along to the next piece of middleware
            // (eventually) it will reach our controllers. If anything inside our Controller,
            // Service, or Repo/Data layers throws an exception - it'll bubble back up here.
            await _next(context);
        }
        catch (Exception e)
        {
            // Here, we will call our logic for handling/routing given specific Exception types
            // to their correct status codes
            await HandleExceptionAsync(context, e);
        }
    }

    // HandleExceptionAsync
    // This method will contain the "logic" for my middleware
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        // Create some way to map specific exception types to HTTP status codes
        // As we grow our app, test it more with a front end, etc - we may encounter
        // exceptions we didn't foresee - and that's okay. But as we find them, we can
        // come back and handle them here in one centralized place.

        int statusCode;

        switch (ex)
        {
            case KeyNotFoundException _:
                statusCode = 404;
                break;
            case DbUpdateException _:
                // Thrown by EF Core when a database constraint is violated.
                // e.g. creating/updating a Course with an invalid InstructorId,
                // or deleting an Instructor that still has Courses assigned (RESTRICT).
                statusCode = 409;
                break;
            case ArgumentOutOfRangeException _:
                statusCode = 400;
                break;
            case ArgumentException _:
                statusCode = 400;
                break;
            case NullReferenceException _:
                statusCode = 404;
                break;
            default:
                statusCode = 500;
                break;
        }

        // Set up our response using the Response object that belongs to context
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        // Create a body, serialized as JSON
        var body = JsonSerializer.Serialize(new
        {
            status = statusCode,
            message = ex.Message
        });

        await context.Response.WriteAsync(body);
    }
}
