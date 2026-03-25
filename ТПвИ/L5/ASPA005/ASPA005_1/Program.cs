using System.Text.RegularExpressions;
using DAL004;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Repository.JSONFileName = "Celebrities.json";
using IRepository repository = Repository.Create("Celebrities");

app.UseExceptionHandler("/Celebrities/Error");

var celebrities = app.MapGroup("/Celebrities");

celebrities.MapGet("", () => Results.Ok(repository.getAllCelebrities()));

celebrities.MapGet("/{id:int}", (int id) =>
{
    Celebrity? celebrity = repository.getCelebrityById(id);
    if (celebrity is null)
    {
        throw new FoundByIdException($"/Celebrities/{id}");
    }

    return Results.Ok(celebrity);
});

celebrities.MapPost("", (Celebrity celebrity) =>
    {
        int? id = repository.addCelebrity(celebrity);
        if (id == null)
            throw new AddCelebrityException("POST /Celebrities error, id == null");
        
        if (repository.SaveChanges() <= 0)
            throw new SaveException("/Celebrities error, SaveChanges() <= 0");

        return new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
    })
    .AddEndpointFilter(async (context, next) =>
    {
        Celebrity? celebrity = context.GetArgument<Celebrity>(0);
        
        if (celebrity == null)
            return Results.Problem("POST /Celebrities error, celebrity == null", statusCode: 500);

        if (string.IsNullOrEmpty(celebrity.Surname) || celebrity.Surname.Length < 2)
            return Results.Conflict("POST /Celebrities error, Surname is null or length < 2");

        return await next(context);
    })
    .AddEndpointFilter(async (context, next) =>
    {
        Celebrity? celebrity = context.GetArgument<Celebrity>(0);
        
        if (celebrity == null)
            return Results.Problem("POST /Celebrities error, celebrity == null", statusCode: 500);

        if (repository.getCelebritiesBySurname(celebrity.Surname).Any())
            return Results.Conflict($"POST /Celebrities error, Surname '{celebrity.Surname}' already exists");

        return await next(context);
    })
    .AddEndpointFilter(async (context, next) =>
    {
        Celebrity? celebrity = context.GetArgument<Celebrity>(0);
        
        if (celebrity == null)
            return Results.Problem("POST /Celebrities error, celebrity == null", statusCode: 500);

        string fileName = Path.GetFileName(celebrity.PhotoPath);
        string fullPath = Path.Combine(repository.BasePath, fileName); 

        if (!File.Exists(fullPath))
        {
            context.HttpContext.Response.Headers.Append("X-Celebrity", $"NotFound = {fileName}");
            return Results.NotFound($"File {fileName} not found in BasePath");
        }

        return await next(context);
    });

celebrities.MapPut("/{id:int}", (int id, Celebrity celebrity) =>
{
    bool updated = repository.updCelebrityById(id, celebrity);
    if (!updated)
    {
        throw new UpdByIdException($"/Celebrities/{id}");
    }

    if (repository.SaveChanges() <= 0)
    {
        throw new SaveException("SaveChanges() <= 0");
    }

    return Results.Ok(repository.getCelebrityById(id));
});

celebrities.MapDelete("/{id:int}", (int id) =>
{
    if (!repository.delCelebrityById(id))
    {
        throw new DelByIdException($"/Celebrities/{id}");
    }

    if (repository.SaveChanges() <= 0)
    {
        throw new SaveException("SaveChanges() <= 0");
    }

    return Results.Ok(new { message = $"Celebrity with Id={id} deleted" });
});

app.Map("/Celebrities/Error", (HttpContext ctx) =>
{
    Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
    if (ex is FoundByIdException or DelByIdException or UpdByIdException)
    {
        return Results.NotFound(ex.Message);
    }

    if (ex is ValidationException or BadHttpRequestException)
    {
        return Results.BadRequest(ex?.Message);
    }

    if (ex is AddCelebrityException or SaveException)
    {
        return Results.Problem(title: "Repository error", detail: ex.Message, statusCode: 500);
    }

    return Results.Problem(title: "Unhandled error", detail: ex?.Message, statusCode: 500);
});

app.MapFallback((HttpContext ctx) =>
    Results.NotFound(new { error = $"Path '{ctx.Request.Path}' is not supported" }));

app.Run();

public sealed class FoundByIdException(string message) : Exception($"Not found: {message}");
public sealed class SaveException(string message) : Exception($"Save error: {message}");
public sealed class AddCelebrityException(string message) : Exception($"Add error: {message}");
public sealed class DelByIdException(string message) : Exception($"Delete error: {message}");
public sealed class UpdByIdException(string message) : Exception($"Update error: {message}");
public sealed class ValidationException(string message) : Exception($"Validation error: {message}");