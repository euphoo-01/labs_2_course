ПИС-3, ПОИТ

**Лабораторная работа 7**

**ASPA** -- приложение ASP.NET CORE

♣ -- задания, требующие самостоятельного изучения (нет в лекциях)

Используйте библиотеку ***DAL_Celebrity_MSSQL*** разработанную в
предыдущем задании.

Оформление html-разметки на усмотрение разработчика

**[Задание 1.]{.underline}**

1.  Разработайте ASPA, применив следующий шаблон.

Имя решения: **ASPA**

Имя проекта: **ASPA007_1**

Версия .NET: **10.0**

![](media/image1.png){width="5.252083333333333in" height="1.0in"}

2.  Приложение **ASPA007_1** должно

2.1. Поддерживать следующий конфигурационный файл

```json
// Celebrities.config.json
// -- PhotosRequestPath - префикс в  URI запроса
// -- PhotosFolder  - директория с папками
// -- ConnectionString - строка подключения к БД
"Celebrities": {
    "PhotosRequestPath": "/Photos",
    "PhotosFolder": "/home/euphoo/02. University/2 КУРС/ТПвИ/L6/ASPA006/Celebrities/",
    "ConnectionString": "Server=localhost;Initial Catalog=LES01;User Id=sa;Password=123;TrustServerCertificate=True"
}
```

2.2. Стартовая страница должна отображать все фотографии

![](media/image3.png){width="4.383753280839895in"
height="2.782609361329834in"}

2.3. При выборе мышью изображения бриллианта отобразить форму для ввода
данных о новой знаменитости, при заполнении и вводе отобразить форму для
подтверждения ввода, при подтверждении добавить данные в БД, а
фотографию в соответствующий директорий.

![](media/image4.png){width="4.478260061242345in"
height="2.7201574803149606in"}

![](media/image5.png){width="4.485705380577428in"
height="2.7478258967629046in"}

![](media/image6.png){width="4.486956474190726in"
height="2.745618985126859in"}

![](media/image7.png){width="4.634782370953631in"
height="2.492037401574803in"}

3.  При выборе мышью фотографии отображается отдельно эта фотография.

![](media/image8.png){width="2.243477690288714in"
height="1.8432228783902012in"}

4.  Ниже отображено примерное содержание файл ***Program.cs***

```cs
using ANC25_WEBAPI_DLL;
using static ANC25_WEBAPI_DLL.CelebritiesAPIExtensions;
internal class Program
{
    private static void Main(string[] args) {
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
    
    app.Run();
    }
}
```

5.  Продемонстрируйте работоспособность приложения.
