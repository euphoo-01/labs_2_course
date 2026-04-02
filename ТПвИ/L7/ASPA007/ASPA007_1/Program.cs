using ANC25_WEBAPI_DLL;
using static ANC25_WEBAPI_DLL.CelebritiesAPIExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddCelebritiesConfiguration();
builder.AddCelebritiesServices();

builder.Services.AddRazorPages();
builder.Services.AddRazorPages(
    o =>
    {
        o.Conventions.AddPageRoute("/Celebrities", "/");
        o.Conventions.AddPageRoute("/NewCelebrity", "/0");
        o.Conventions.AddPageRoute("/Celebrity", "/Celebrities/{id:int:min(1)}");
        o.Conventions.AddPageRoute("/Celebrity", "/{id:int:min(1)}");
    }
);

var app = builder.Build();
app.UseStaticFiles();

app.UseANCErrorHandler("ANC27X");

if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Error"); }

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.MapCelebrities();
app.MapLifeevents();
app.MapPhotoCelebrities();

using (var scope = app.Services.CreateScope()) {
    var config = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<CelebritiesConfig>>().Value;
    new DAL_Celebrity_MSSQL.Init(config.ConnectionString);
    DAL_Celebrity_MSSQL.Init.Execute(delete: false, create: true);
}

app.Run();

