using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging((options) =>
{
    options.LoggingFields = HttpLoggingFields.All;
});

var app = builder.Build();

app.UseHttpLogging();

app.MapGet("/", () => Results.Content("<html><h1 style='color: red;'>Моя первая ASPA</h1></html>", "text/html; charset=utf-8"));
app.MapGet("/profile", (string user) => Results.Content($"<html><h1 style='color: red;'>Hello, {user}</h1></html>", "text/html; charset=utf-8"));
app.Run();