using DAL_Celebrity;
using DAL_Celebrity_MSSQL;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ANC25_WEBAPI_DLL;

public class CelebritiesConfig {
    public string PhotosRequestPath { get; set; } = string.Empty;
    public string PhotosFolder { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}

public static class CelebritiesAPIExtensions {
    public static void AddCelebritiesConfiguration(this WebApplicationBuilder builder) {
        builder.Configuration.AddJsonFile("Celebrities.config.json", optional: false, reloadOnChange: true);
        builder.Services.Configure<CelebritiesConfig>(builder.Configuration.GetSection("Celebrities"));
    }

    public static void AddCelebritiesServices(this WebApplicationBuilder builder) {
        builder.Services.AddScoped<IRepository>(p => {
            var config = p.GetRequiredService<IOptions<CelebritiesConfig>>().Value;
            return new Repository(config.ConnectionString);
        });
    }

    public static void UseANCErrorHandler(this IApplicationBuilder app, string code) {
        app.Use(async (context, next) => {
            try {
                await next();
            } catch (Exception ex) {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { error = code, detail = ex.Message });
            }
        });
    }

    public static void MapCelebrities(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/api/Celebrities");
        group.MapGet("/", (IRepository repo) => repo.GetAllCelebrities());
        group.MapGet("/{id:int}", (IRepository repo, int id) => {
            var c = repo.GetCelebrityById(id);
            return c != null ? Results.Ok(c) : Results.NotFound();
        });
        group.MapPost("/", (IRepository repo, Celebrity c) => {
            repo.AddCelebrity(c);
            return Results.Created($"/api/Celebrities/{c.Id}", c);
        });
        group.MapDelete("/{id:int}", (IRepository repo, int id) => 
            repo.DelCelebrity(id) ? Results.Ok() : Results.NotFound());
    }

    public static void MapLifeevents(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/api/Lifeevents");
        group.MapGet("/", (IRepository repo) => repo.GetAllLifeevents());
        group.MapGet("/Celebrities/{id:int}", (IRepository repo, int id) => 
            repo.GetLifeeventsByCelebrityId(id));
    }

    public static void MapPhotoCelebrities(this IEndpointRouteBuilder endpoints) {
        using (var scope = endpoints.ServiceProvider.CreateScope()) {
            var config = scope.ServiceProvider.GetRequiredService<IOptions<CelebritiesConfig>>().Value;
            var requestPath = config.PhotosRequestPath.TrimEnd('/') + "/{fname}";
            endpoints.MapGet(requestPath, (string fname, IOptions<CelebritiesConfig> cfg) => {
                var path = Path.Combine(cfg.Value.PhotosFolder, fname);
                return File.Exists(path) ? Results.File(path, "image/jpeg") : Results.NotFound();
            });
        }
    }
}
