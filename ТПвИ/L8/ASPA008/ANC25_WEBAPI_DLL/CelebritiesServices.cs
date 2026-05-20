using DAL_Celebrity;
using DAL_Celebrity_MSSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ANC25_WEBAPI_DLL;

public sealed class CelebritiesConfig
{
    public string PhotosRequestPath { get; set; } = "/Photos";
    public string PhotosFolder { get; set; } = "Photos";
    public string ConnectionString { get; set; } = string.Empty;
    public string CountryCodesFile { get; set; } = "CountryCodes/iso3166-1-alpha2-country-codes.json";
}

public sealed class CountryCode
{
    public string CountryLabel { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public interface ICountryCodes
{
    IReadOnlyList<CountryCode> GetAll();
    string GetLabel(string code);
}

public sealed class CountryCodes : ICountryCodes
{
    private readonly Lazy<IReadOnlyList<CountryCode>> codes;

    public CountryCodes(IOptions<CelebritiesConfig> options)
    {
        var file = options.Value.CountryCodesFile;
        codes = new Lazy<IReadOnlyList<CountryCode>>(() =>
        {
            if (!File.Exists(file))
            {
                return Array.Empty<CountryCode>();
            }

            using var stream = File.OpenRead(file);
            return JsonSerializer.Deserialize<List<CountryCode>>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })?.OrderBy(c => c.CountryLabel).ToList() ?? [];
        });
    }

    public IReadOnlyList<CountryCode> GetAll() => codes.Value;

    public string GetLabel(string code) =>
        GetAll().FirstOrDefault(c => string.Equals(c.Code, code, StringComparison.OrdinalIgnoreCase))?.CountryLabel
        ?? code;
}

public sealed class CelebrityTitles
{
    public string Index { get; set; } = "Celebrities";
    public string Create { get; set; } = "New Celebrity";
    public string Details { get; set; } = "Celebrity";
    public string Edit { get; set; } = "Edit Celebrity";
    public string Delete { get; set; } = "Delete Celebrity";
}

public static class CelebritiesAPIExtensions
{
    public static void AddCelebritiesConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("Celebrities.config.json", optional: false, reloadOnChange: true);
        builder.Services.Configure<CelebritiesConfig>(builder.Configuration.GetSection("Celebrities"));
        builder.Services.Configure<CelebrityTitles>(builder.Configuration.GetSection("CelebrityTitles"));
    }

    public static void AddCelebritiesServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRepository>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<CelebritiesConfig>>().Value;
            return Repository.Create(config.ConnectionString);
        });
        builder.Services.AddSingleton<ICountryCodes, CountryCodes>();
    }

    public static void MiddlewareErrorHandler(this IApplicationBuilder app, string code = "ASPA008")
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = code, detail = ex.Message });
            }
        });
    }
}
