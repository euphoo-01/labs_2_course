using System.Text.RegularExpressions;
using DAL004; 
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Repository.JSONFileName = "Celebrities.json";
using IRepository repository = Repository.Create("Celebrities");

Validation.SurnameFilter.repository = Validation.PhotoExistFilter.repository = repository;

PutIdFilter.repository = repository;
DeleteIdFilter.repository = repository;

app.UseExceptionHandler("/Celebrities/Error");

RouteGroupBuilder api = app.MapGroup("/Celebrities");

api.MapGet("", () => Results.Ok(repository.getAllCelebrities()));

api.MapGet("/{id:int}", (int id) =>
{
    Celebrity? celebrity = repository.getCelebrityById(id);
    if (celebrity is null) throw new FoundByIdException($"/Celebrities/{id}");
    return Results.Ok(celebrity);
});

api.MapPost("", (Celebrity celebrity) =>
{
    int? id = repository.addCelebrity(celebrity with { id = 0 });
    if (id is null) throw new AddCelebrityException("id is null");
    if (repository.SaveChanges() <= 0) throw new SaveException("SaveChanges() <= 0");

    return Results.Ok(new Celebrity(id.Value, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath));
})
.AddEndpointFilter<Validation.SurnameFilter>()
.AddEndpointFilter<Validation.PhotoExistFilter>();

api.MapPut("/{id:int}", (int id, Celebrity celebrity) =>
{
    bool updated = repository.updCelebrityById(id, celebrity);
    if (!updated) throw new UpdByIdException($"/Celebrities/{id}");
    if (repository.SaveChanges() <= 0) throw new SaveException("SaveChanges() <= 0");

    return Results.Ok(repository.getCelebrityById(id));
})
.AddEndpointFilter<PutIdFilter>();

api.MapDelete("/{id:int}", (int id) =>
{
    if (!repository.delCelebrityById(id)) throw new DelByIdException($"/Celebrities/{id}");
    if (repository.SaveChanges() <= 0) throw new SaveException("SaveChanges() <= 0");

    return Results.Ok(new { message = $"Celebrity with Id={id} deleted" });
})
.AddEndpointFilter<DeleteIdFilter>();

app.Map("/Celebrities/Error", (HttpContext ctx) =>
{
    Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
    if (ex is FoundByIdException or DelByIdException or UpdByIdException)
        return Results.NotFound(ex.Message);

    if (ex is ValidationException or BadHttpRequestException)
        return Results.BadRequest(ex?.Message);

    if (ex is AddCelebrityException or SaveException)
        return Results.Problem(title: "Repository error", detail: ex.Message, statusCode: 500);

    return Results.Problem(title: "Unhandled error", detail: ex?.Message, statusCode: 500);
});

app.MapFallback((HttpContext ctx) =>
    Results.NotFound(new { error = $"Path '{ctx.Request.Path}' is not supported" }));

app.Run();

public static class Validation
{
    public class SurnameFilter : IEndpointFilter
    {
        public static IRepository? repository;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            Celebrity? model = context.GetArgument<Celebrity>(0);
            if (model is null) throw new ValidationException("celebrity model is null");

            if (string.IsNullOrWhiteSpace(model.Firstname) || string.IsNullOrWhiteSpace(model.Surname))
                throw new ValidationException("Firstname/Surname are required");

            if (!Regex.IsMatch(model.Firstname, "^[A-Za-z\\-]{2,40}$") || !Regex.IsMatch(model.Surname, "^[A-Za-z\\-]{2,40}$"))
                throw new ValidationException("Firstname/Surname must contain only letters and '-'");

            if (repository is null) throw new ValidationException("Repository is not initialized");

            bool duplicated = repository.getCelebritiesBySurname(model.Surname).Any();
            if (duplicated) throw new ValidationException($"Celebrity with surname '{model.Surname}' already exists");

            return await next(context);
        }
    }

    public class PhotoExistFilter : IEndpointFilter
    {
        public static IRepository? repository;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            Celebrity? model = context.GetArgument<Celebrity>(0);
            
            if (string.IsNullOrWhiteSpace(model?.PhotoPath) || !Regex.IsMatch(model.PhotoPath, "^/Photo/[A-Za-z0-9\\-]+\\.(jpg|jpeg|png)$", RegexOptions.IgnoreCase))
            {
                throw new ValidationException("PhotoPath must match /Photo/<name>.jpg|jpeg|png");
            }

            return await next(context);
        }
    }
}

public sealed class PutIdFilter : IEndpointFilter
{
    public static IRepository? repository;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        int id = context.GetArgument<int>(0);
        if (id <= 0) throw new ValidationException("Id must be > 0");
        if (repository?.getCelebrityById(id) is null) throw new UpdByIdException($"/Celebrities/{id}");

        return await next(context);
    }
}

public sealed class DeleteIdFilter : IEndpointFilter
{
    public static IRepository? repository;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        int id = context.GetArgument<int>(0);
        if (id <= 0) throw new ValidationException("Id must be > 0");
        if (repository?.getCelebrityById(id) is null) throw new DelByIdException($"/Celebrities/{id}");

        return await next(context);
    }
}

public sealed class FoundByIdException(string message) : Exception($"Not found: {message}");
public sealed class SaveException(string message) : Exception($"Save error: {message}");
public sealed class AddCelebrityException(string message) : Exception($"Add error: {message}");
public sealed class DelByIdException(string message) : Exception($"Delete error: {message}");
public sealed class UpdByIdException(string message) : Exception($"Update error: {message}");
public sealed class ValidationException(string message) : Exception($"Validation error: {message}");