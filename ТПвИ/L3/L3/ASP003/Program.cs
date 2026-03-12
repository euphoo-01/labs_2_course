using DAL003;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDirectoryBrowser();

var app = builder.Build();

var celebritiesPath = Path.Combine(builder.Environment.ContentRootPath, "Celebrities");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(celebritiesPath),
    RequestPath = "/Photo"
});



app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(celebritiesPath),
    RequestPath = "/Celebrities/download"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(celebritiesPath),
    RequestPath = "/Celebrities/download",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Content-Disposition", "attachment; filename=\"" + ctx.File.Name + "\"");
    }
});

Repository.JSONFileName = "Celebrities.json";
using (IRepository repository = Repository.Create("Celebrities"))
{
    app.MapGet("/Celebrities", () => repository.getAllCelebrities());
    app.MapGet("/Celebrities/{id:int}", (int id) => repository.getCelebrityById(id));
    app.MapGet("/Celebrities/BySurname/{surname}", (string surname) => repository.getCelebritiesBySurname(surname));
    app.MapGet("/", () => "Hello world!");
    app.Run();
}
