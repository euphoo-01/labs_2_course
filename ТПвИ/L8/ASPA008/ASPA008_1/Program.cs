using ANC25_WEBAPI_DLL;
using ASPA008_1.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.AddCelebritiesConfiguration();
builder.AddCelebritiesServices();
builder.Services.AddScoped<WikipediaLinksFilter>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<WikipediaLinksFilter>();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Celebrities/Error");
    app.UseHsts();
}

app.MiddlewareErrorHandler();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapGet("/Photos/{fileName}", (string fileName, Microsoft.Extensions.Options.IOptions<CelebritiesConfig> options) =>
{
    var configuredFolder = options.Value.PhotosFolder;
    var fullPath = Path.IsPathRooted(configuredFolder)
        ? Path.Combine(configuredFolder, fileName)
        : Path.Combine(app.Environment.ContentRootPath, configuredFolder, fileName);

    return File.Exists(fullPath) ? Results.File(fullPath, "image/jpeg") : Results.NotFound();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Celebrities}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
