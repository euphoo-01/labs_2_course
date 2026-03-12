> Примечание! Проект предыдущей лабораторной работы находится в папке `./L3/`. Там находится вся нужная информация. Он доступен только для чтения и нужен чтобы продублировать необходимый код в текущую лабораторную работу и расширить его.

ПИС-3, ПОИТ

**Лабораторная работа 4**

**ASPA** -- приложение ASP.NET CORE

♣ -- задания, требующие самостоятельного изучения (нет в лекциях)

♣**[Задание 1.]{.underline}**

1.  Установите программную платформу **POSTMAN**
    <https://www.postman.com/>
    <https://habr.com/ru/companies/maxilect/articles/596789/>

2.  Убедитесь в работоспособности платформы.

(можно пропустить. Я использую bruno)

**[Задание 2.]{.underline}**

3.  Разработайте библиотеку, применив следующий шаблон (проект уже создан, нужно написать реализацию)

Имя решения: **ASPA**

Имя проекта: **DAL004**

Версия .NET: **7.0** или **8.0**

![](media/image1.png){width="5.283333333333333in"
height="1.0847222222222221in"}

4.  Библиотека **DAL004** должна реализовать **DAL** (Data Access Level)
    реализовав паттерн **Repository** для доступа к следующим формате
    JSON (формат, данные и принцип их размещения описаны в лабораторной
    работе 3).

5.  **Repository** должен реализовывать следующий интерфейс (расширен
    относительно **DAL003**).

```cs
// Строки, расширяющие функционал. Интерфейс из прошлой лабораторной работы расшить этими строками
int? addCelebrity(Celebrity celebrity);
bool delCelebrityById(int id);
int? updCelebrityById(int id, Celebrity celebrity);
int SaveChanges();
```

![](media/image2.emf){width="7.268055555555556in"
height="1.6640715223097113in"}

6.  Разработайте консольное приложение, применив следующий шаблон

Имя решения: **ASPA**

Имя проекта: **Test**\_**DAL004**

Версия .NET: **7.0** или **8.0**

![](media/image3.png){width="5.4905653980752405in"
height="0.9905664916885389in"}

7.  Приложение **Test**\_**DAL004** предназначено для тестирования новых
    функций **DAL004.** Ниже приведён код и результат теста

```cs
Repository.JSONFileName = "Celebrities.json";
using (IRepository repository = Repository.Create("Celebrities"))
{
    void print (string label) {
        Console.WriteLine("--- "+label+" -------------");
        foreach (Celebrity celebrity in repository.getAllCelebrities()) {
            Console.WriteLine($"Id = {celebrity.id}, Firstname = {celebrity.Firstname}, " + $"Surname = {celebrity.Surname}, PhotoPath = {celebrity.PhotoPath} ");
        }
    }
};

print("start");

int? testdel1 = repository.addCelebrity(new Celebrity(0, "TestDel1", "TestDel1", "Photo/TestDel1.jpg"));
int? testdel2 = repository.addCelebrity(new Celebrity(0, "TestDel2", "TestDel2", "Photo/TestDel2.jpg"));
int? testupd1 = repository.addCelebrity(new Celebrity(0, "TestUpd1", "TestUpd1", "Photo/TestUpd1.jpg"));
int? testupd2 = repository.addCelebrity(new Celebrity(0, "TestUpd2", "TestUpd2", "Photo/TestUpd2.jpg"));
repository.SaveChanges();
print("add 4");

if (testdel1 != null)
    if (repository.delCelebrityById((int) testdel1)) Console.WriteLine($" delete {testdel1} "); else Console.WriteLine($"delete {testdel1} error");
if (testdel2 != null)
    if (repository.delCelebrityById((int) testdel2)) Console.WriteLine($" delete {testdel2} "); else Console.WriteLine($"delete {testdel2} error");
if (repository.delCelebrityById(1000)) Console.WriteLine($" delete {1000} "); else Console.WriteLine($"delete {1000} error");
repository.SaveChanges();
print("del 2");

if (testupd1 != null) {
    if (repository.updCelebrityById((int) testupd1, new Celebrity(0, "Updated1", "Updated1", "Photo/Updated1.jpg"))) Console.WriteLine($" update {testupd1} ");
    else Console.WriteLine($"update {testupd1} error");
}
if (testupd2 != null) {
    if (repository.updCelebrityById((int) testupd2, new Celebrity(0, "Updated2", "Updated2", "Photo/Updated2.jpg"))) Console.WriteLine($" update {testupd2} ");
    else Console.WriteLine($"update {testupd2} error");
}
if (repository.updCelebrityById(1000, new Celebrity(0, "Updated1000", "Updated1000", "Photo/Updated1000.jpg"))) Console.WriteLine($" update {1000} ");
else Console.WriteLine($" update {1000} error");
repository.SaveChanges();
print("upd 2");
```

![](media/image4.png){width="6.93614501312336in"
height="4.388058836395451in"}

![](media/image5.png){width="6.940298556430446in"
height="5.298507217847769in"}

8.  Выполните тест **Test**\_**DAL004** и убедитесь в его успешном
    выполнении.

**[Задание 3.]{.underline}**

9.  Разработайте ASPA, применив следующий шаблон.

Имя решения: **ASPA**

Имя проекта: **ASPA004_1**

Версия .NET: **7.0** или **8.0**

![](media/image6.png){width="5.517243000874891in"
height="1.1509426946631671in"}

10. Подключите библиотеку **DAL004** к **ASPA004_1**

11. Исследуйте приведённый ниже фрагмент приложения и включите в
    **ASPA004_1.** Поясните назначение подчёркнутых или обведённых
    фрагментов программного кода.

```cs
Repository.JSONFileName = "Celebrities.json";
using (IRepository repository = Repository.Create("Celebrities"))
{
    app.UseExceptionHandler("/Celebrities/Error");
    
    app.MapGet("/Celebrities", () => repository.getAllCelebrities());
    app.MapGet("/Celebrities/{id:int}", (int id) => {
        Celebrity? celebrity = repository.getCelebrityById(id);
        if (celebrity == null) throw new FoundByIdException($"Celebrity Id = {id}");
        return celebrity;
    });
    app.MapPost("/Celebrities", (Celebrity celebrity) => {
        int? id = repository.addCelebrity(celebrity);
        if (id == null) throw new AddCelebrityException("/Celebrities error, id == null");
        if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
        return new Celebrity((int) id, celebrity.Surname, celebrity.PhotoPath);
    });
    
    app.MapFallback((HttpContext ctx) => Results.NotFound(new {error = $"path {ctx.Request.Path} not supported"}));
    
    app.Map("/Celebrities/Error", (HttpContext ctx) => {
        Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
        IResult rc = Results.Problem(detail, "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);
        if (ex != null) {
            if (ex is FoundByIdException) rc = Results.NotFound(ex.Message);
            if (ex is BadHttpRequestException) rc = Results.BadRequest(ex.Message);
            if (ex is SaveException) rc = Results.Problem(title: "ASPA004/SaveChanges", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
            if (ex is AddCelebrityException) rc = Results.Problem(title: "ASPA004/addCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
        }
    })
};

public class FoundByIdException:Exception {public FoundByIdException(string message) : base($"Found by Id: {message}"){ } };
public class SaveException:Exception {public SaveException(string message) : base($"SaveChanges error: {message}") {} };
public class AddCelebrityException:Exception {public AddCelebrityException(string message) : base($"AddCelebrityException error: {message}") {} };
```

![](media/image7.emf){width="7.380597112860892in"
height="3.985074365704287in"}

12. Выполните следующие запросы с помощью **POSTMAN** и сравните
    полученные ответы. Поясните: какие фрагменты кода принимали участие
    в обработке каждого запроса и формировании ответа? Определите статус
    каждого ответа в Postman, а также где он задаётся в программном коде
    и формируется соответствующее сообщение.

![](media/image8.png){width="6.492004593175853in"
height="3.858208661417323in"}![](media/image9.png){width="6.492537182852144in"
height="2.2761198600174977in"}

13. 

![](media/image10.png){width="6.43283573928259in"
height="3.0223873578302713in"}

14. 

![](media/image11.emf){width="6.43283573928259in"
height="2.246268591426072in"}

15. 

![](media/image12.png){width="6.43283573928259in"
height="2.6119411636045493in"}

16. 

![](media/image13.png){width="6.455223097112861in"
height="4.462687007874016in"}

17. 

![](media/image14.png){width="6.41044728783902in"
height="2.888059930008749in"}

18. 

![](media/image15.png){width="6.43283573928259in"
height="2.2835826771653545in"}

19. 

![](media/image16.png){width="6.492537182852144in"
height="2.3656714785651793in"}

20. Не останавливая **ASPA004_1**,выполните действия, приводящие к
    следующему поведению **ASPA004_1.**

![](media/image17.png){width="6.447760279965005in"
height="4.522387357830271in"}

21. Выполните анализ протокола, выводимого на консоль в предыдущем
    пункте задания. Предложите такие изменение в программном коде, чтобы
    в поле **detail**, возвращаемого в ответе сообщения, была более
    подробная информация об ошибке Измените программный код
    **ASPA004_1** так**,** чтобы поведение **ASPA004_1** стало примерно
    как в следующем примере.

![](media/image18.png){width="6.559701443569554in"
height="3.8283573928258967in"}

**[Задание 3.]{.underline}**

22. Разработайте ASPA, применив следующий шаблон.

Имя решения: **ASPA**

Имя проекта: **ASPA004_2**

Версия .NET: **7.0** или **8.0**

![](media/image6.png){width="5.517243000874891in"
height="1.1509426946631671in"}

23. Подключите библиотеку **DAL004** к **ASPA004_2.**

24. Продублируйте программный код файла **Program.cs** приложения
    **ASPA004_1** в одноименный файл приложения **ASPA004_2.**

25. Добавьте в **Program.cs** фрагмент кода для обработки
    DELETE-запросов. Зачёркнутый код должен быть написан вами.

```cs
app.MapDelete("/Celebrities/{id:int}", (int id) => {});
```

![](media/image19.png){width="7.216417322834646in"
height="0.6119411636045494in"}

26. Следующие два фрагмента программного кода уже существуют в
    **Program.cs.** Необходимо вставить собственный программный код
    вместо зачёркнутых строк

```cs
app.Map("/Celebrities/Error", (HttpContext ctx) => {
    Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
    IResult rc = Results.Problem(detail: "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);
    if (ex != null) {
        // Строчка в примере закрашена
        if (ex is FileNotFoundException) rc = Results.Problem(title: "ASPA00", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
        if (ex is FoundByIdException) rc = Results.NotFound(ex.Message);
        if (ex is BadHttpRequestException) rc = Results.BadRequest(ex.Message);
        if (ex is SaveException) rc = Results.Problem(title: "ASPA004/SaveChanges", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
        if (ex is AddCelebrityException) rc = Results.Problem(title: "ASPA004/addCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
    }
})

// Зачеркнутая строка
public class FoundByIdException:Exception {public FoundByIdException(string message) : base($"Found by Id: {message}"){ } };
public class SaveException:Exception {public SaveException(string message) : base($"SaveChanges error: {message}") {} };
public class AddCelebrityException:Exception {public AddCelebrityException(string message) : base($"AddCelebrityException error: {message}") {} };
```

![](media/image20.png){width="7.261111111111111in" height="1.30625in"}

![](media/image21.png){width="7.261111111111111in"
height="0.5673611111111111in"}

27. Окончательный вариант **ASPA004_2** должен обеспечивать успешное
    прохождение следующих 5 тестов.

![](media/image22.emf){width="6.127083333333333in"
height="4.888194444444444in"}

![](media/image23.emf){width="7.253472222222222in" height="3.19375in"}

![](media/image24.png){width="4.805970034995625in"
height="3.1641786964129484in"}

![](media/image25.png){width="5.953712817147856in"
height="3.1343285214348207in"}

![](media/image26.png){width="6.359020122484689in"
height="2.9701487314085737in"}

**[Задание 4]{.underline}**

28. Разработайте ASPA, применив следующий шаблон.

Имя решения: **ASPA**

Имя проекта: **ASPA004_3**

Версия .NET: **7.0** или **8.0**

![](media/image6.png){width="5.517243000874891in"
height="1.1509426946631671in"}

29. Подключите библиотеку **DAL004** к **ASPA004_3.**

30. Продублируйте программный код файла **Program.cs** приложения
    **ASPA004_2** в одноименный файл приложения **ASPA004_3.**

31. Добавьте в **Program.cs** фрагмент кода для обработки PUT-запросов.
    Зачёркнутый код должен быть написан вами.

```cs
app.MapPut("/Celebrities/{id:int}", (int id, Celebrity celebrity) => {
    // Код закрашен
})
```

![](media/image27.png){width="7.25625in" height="0.9680555555555556in"}

32. Другие два фрагмента, в которые необходимо дописать код совпадают с
    фрагментами, используемыми при разработке **ASPA004_2.**

33. Окончательный вариант **ASPA004_3** должен обеспечивать успешное
    прохождение следующих 3 тестов.

![](media/image28.png){width="6.19200021872266in"
height="4.063999343832021in"}

![](media/image29.png){width="6.584027777777778in"
height="4.952083333333333in"}

![](media/image30.png){width="6.576000656167979in"
height="3.743798118985127in"}

**[Задание 5.]{.underline}Ответьте на следующие вопросы**

34. Поясните назначение следующих функций

![](media/image31.png){width="5.647916666666666in"
height="1.6319444444444444in"}

35. Поясните назначение следующих объектов и интерфейсов

![](media/image32.png){width="4.040277777777778in"
height="1.1680555555555556in"}

36. ♣Чем известны Ada Lovelace и Charles Babbage?☺
