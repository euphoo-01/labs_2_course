# ASPA008: теория к защите

## Что сделано в лабораторной

Разработано ASP.NET Core MVC-приложение `ASPA008_1`, которое работает с базой знаменитостей через библиотеку `DAL_Celebrity_MSSQL` из предыдущих лабораторных. Приложение показывает список знаменитостей, карточку выбранной знаменитости, жизненные события, форму добавления с подтверждением, редактирование и удаление с подтверждением.

Проект собран под `net10.0`, потому что в текущей среде установлены SDK/runtime .NET 9/10, а соседние лабораторные уже используют `net10.0`. По архитектуре работа соответствует заданию для .NET 7/8.

## MVC в ASP.NET Core

MVC разделяет приложение на три части:

- `Model` хранит данные и правила предметной области. В работе это сущности `Celebrity`, `Lifeevent` и view-model классы.
- `View` отвечает за HTML-разметку. В работе это файлы в `Views/Celebrities`.
- `Controller` принимает HTTP-запрос, вызывает модель/сервисы и выбирает представление. В работе это `CelebritiesController`.

Главная идея MVC: контроллер не должен содержать HTML, а представление не должно напрямую работать с базой данных.

## Контроллер

`CelebritiesController` содержит actions:

- `Index` отображает все фотографии знаменитостей.
- `Details` показывает данные выбранной знаменитости и события жизни.
- `Create`, `ConfirmCreate`, `CreateConfirmed` реализуют добавление с подтверждением.
- `Edit` изменяет данные знаменитости.
- `Delete`, `DeleteConfirmed` удаляют запись с подтверждением.

Доступ к данным идет через интерфейс `IRepository`, который внедряется через DI-контейнер.

## Dependency Injection

DI используется для передачи зависимостей без ручного создания объектов внутри классов. В `Program.cs` регистрируются:

- `IRepository` как репозиторий базы данных;
- `ICountryCodes` как сервис ISO-кодов стран;
- `WikipediaLinksFilter` как action-фильтр;
- `IOptions<CelebritiesConfig>` как конфигурация приложения.

Это упрощает замену реализации и тестирование.

## Конфигурация

Файл `Celebrities.config.json` содержит параметры:

- путь запроса для фото: `/Photos`;
- папку с фото: `Photos`;
- путь к JSON-файлу ISO-кодов;
- строку подключения к MSSQL;
- заголовки представлений.

Конфигурация подключается через `builder.Configuration.AddJsonFile(...)` и читается через `IOptions<CelebritiesConfig>`.

## Сервис CountryCodes

`CountryCodes` читает JSON-файл `CountryCodes/iso3166-1-alpha2-country-codes.json`, преобразует его в список объектов `CountryCode` и предоставляет методы:

- `GetAll()` для заполнения dropdownlist;
- `GetLabel(code)` для получения названия страны по коду.

В partial view сервис внедряется директивой `@inject`.

## Partial View

Partial view `_CelebrityForm.cshtml` содержит общую форму для создания и редактирования знаменитости. Это убирает дублирование HTML.

В форме используются HTML helpers:

- `Html.BeginForm` для генерации `<form>`;
- `Html.TextBoxFor` для текстовых полей;
- `Html.DropDownListFor` для выбора национальности;
- `Html.ValidationMessageFor` для сообщений валидации.

## HTML-helper

HTML-helper `CelebrityFoto` находится в `Helpers/CelebrityHtmlHelpers.cs`. Он формирует тег `<img>` для фотографии знаменитости.

Преимущество helper-а: логика построения одинакового HTML вынесена из представлений и переиспользуется в `Index` и `Details`.

## Action-фильтр

`WikipediaLinksFilter` выполняется после action-метода. Если view-model является `CelebrityDetailsViewModel`, фильтр добавляет ссылки на Wikipedia по имени знаменитости.

Action-фильтры применяются для поперечной логики, которую не хочется дублировать в каждом action: логирование, проверка, подготовка дополнительных данных, обработка результата.

## Layout

`Views/Shared/_Layout.cshtml` задает общий макет страниц: подключение CSS/JS, навигацию и место для `@RenderBody()`.

Layout нужен, чтобы не повторять одну и ту же HTML-обвязку в каждом представлении.

## Маршрутизация

В `Program.cs` задан стандартный MVC-маршрут:

```csharp
{controller=Celebrities}/{action=Index}/{id?}
```

Поэтому корневой адрес приложения открывает `CelebritiesController.Index`.

## Middleware

Middleware обрабатывает HTTP-запросы последовательно в pipeline. В работе добавлен `MiddlewareErrorHandler`, который перехватывает исключения и возвращает JSON с кодом ошибки.

Также используются стандартные middleware: routing, authorization, static assets.

## Как объяснить добавление знаменитости

Пользователь нажимает бриллиант на главной странице. Открывается форма `Create`, где вводятся имя, национальность из dropdownlist и имя файла фото. POST-запрос идет в `ConfirmCreate`; если модель валидна, показывается страница подтверждения. После подтверждения `CreateConfirmed` добавляет объект `Celebrity` через `IRepository`.

## Как проверить

```bash
dotnet build ASPA008/ASPA.slnx
dotnet run --project ASPA008/ASPA008_1/ASPA008_1.csproj
```

Для полноценной работы с данными нужен доступный MSSQL с базой `LES01` и строкой подключения из `Celebrities.config.json`.
