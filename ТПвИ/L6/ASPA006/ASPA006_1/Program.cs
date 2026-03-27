using DAL_Celebrity;
using DAL_Celebrity_MSSQL;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Celebrities.config.json");
builder.Services.Configure<CelebritiesConfig>(builder.Configuration.GetSection("Celebrities"));

builder.Services.AddScoped<IRepository>(p => {
    var config = p.GetRequiredService<IOptions<CelebritiesConfig>>().Value;
    return new Repository(config.ConnectionString);
});

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

var celebs = app.MapGroup("/api/Celebrities");
celebs.MapGet("/", (IRepository repo) => repo.GetAllCelebrities());
celebs.MapGet("/{id:int:min(1)}", (IRepository repo, int id) => {
    var c = repo.GetCelebrityById(id);
    return c != null ? Results.Ok(c) : Results.NotFound(new { detail = $"Celebrity Id = {id}" });
});
celebs.MapDelete("/{id:int:min(1)}", (IRepository repo, int id) => repo.DelCelebrity(id) ? Results.Ok() : Results.NotFound());
celebs.MapPost("/", (IRepository repo, Celebrity c) => { repo.AddCelebrity(c); return Results.Created($"/api/Celebrities/{c.Id}", c); });

app.MapGet("/api/photo/{fname}", (string fname, IOptions<CelebritiesConfig> config) => {
    var path = Path.Combine(config.Value.PhotosFolder, fname);
    return File.Exists(path) ? Results.File(path, "image/jpeg") : Results.NotFound();
});

var events = app.MapGroup("/api/Lifeevents");
events.MapGet("/", (IRepository repo) => repo.GetAllLifeevents());
events.MapGet("/Celebrities/{id:int:min(1)}", (IRepository repo, int id) => repo.GetLifeeventsByCelebrityId(id));

app.Run();

public class CelebritiesConfig {
    public string PhotosFolder { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}