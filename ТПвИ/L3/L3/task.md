ПИС-3, ПОИТ

**Лабораторная работа 3**

**ASPA** -- приложение ASP.NET CORE

♣ -- задания, требующие самостоятельного изучения (нет в лекциях)

♣**[Задание 1.]{.underline}**

1.  Разработайте библиотеку, применив следующий шаблон (проект библиотеки уже создан)

Имя решения: **ASPA**

Имя проекта: **DAL003**

Версия .NET: **10.0**

Библиотека классов .NET

2.  Библиотека **DAL003** должна реализовать DAL (Data Access Level)
    реализовав паттерн **Repository** для доступа к следующим данным в
    формате JSON.

Фото находятся в папке Celebrities и Celebrities.json

3.  JSON-файл ( статическое свойство ***Repository.JSONFileName***) и
    фотографии, находятся в директории, имя которого задается параметром
    конструктора ***Repository***. Сам директорий с файлами фотографий и
    JSON-файлом располагается в текущем директории приложения.

4.  Структура директории для хранения фотографий и json-файла приведена
    ниже

![](media/image3.png){width="5.603472222222222in"
height="2.5569444444444445in"}

5.  **Repository** должен реализовывать следующий интерфейс

```cs
public interface IRepository:IDisposable{
    string BasePath {get;}
    Celebrity[] getAllCelebrities();
    Celebrity? getCelebrityById(int id);
    Celebrity[] getCelebrityBySurname(string Surname);
    string? getPhotoPathById(int id);
}

public record Celebrity (int id, string firstname, string surname, string photoPath);
```

♣**[Задание 2.]{.underline}**

6.  Разработайте консольное приложение, применив следующий шаблон (проект консольного приложения уже создан)

Имя решения: **ASPA**

Имя проекта: **Test**\_**DAL003**

Версия .NET: **10.0**

![](media/image5.png){width="5.4905653980752405in"
height="0.9905664916885389in"}

7.  Приложение **Test**\_**DAL003** предназначено для тестирования
    **DAL003.**

8.  Ниже приведён код и результат теста.

```cs
private static void Main(string[] args) {
    Repository.JSONFileName = 'Celebrities.json';
    using (IRepository repository in Repository.Create("Celebrities")) {
        foreach (Celebrity celebrity in repository.getAllCelebrities())
        {
            Console.WriteLine($"Id = {celebrity.id}, Firstname = {celebrity.Firstname}, " + $"Surname = {celebrity.surname}, PhotoPath = {celebrity.PhotoPath} ");
        }
        Celebrity? celebrity1 = repository.getCelebrityById(1);
        if (celebrity1 != null) {
            Console.WriteLine($"Id = {celebrity1.id}, Firstname = {celebrity1.Firstname}, " + $"Surname = {celebrity1.surname}, PhotoPath = {celebrity1.PhotoPath} ");
        }
        Celebrity? celebrity2 = repository.getCelebrityById(2);
        if (celebrity2 != null) {
            Console.WriteLine($"Id = {celebrity2.id}, Firstname = {celebrity2.Firstname}, " + $"Surname = {celebrity2.surname}, PhotoPath = {celebrity2.PhotoPath} ");
        }
        Celebrity? celebrity3 = repository.getCelebrityById(3);
        if (celebrity3 != null) {
            Console.WriteLine($"Id = {celebrity3.id}, Firstname = {celebrity3.Firstname}, " + $"Surname = {celebrity3.surname}, PhotoPath = {celebrity3.PhotoPath} ");
        }
        Celebrity? celebrity7 = repository.getCelebrityById(7);
        if (celebrity7 != null) {
            Console.WriteLine($"Id = {celebrity7.id}, Firstname = {celebrity7.Firstname}, " + $"Surname = {celebrity7.surname}, PhotoPath = {celebrity7.PhotoPath} ");
        }
        Celebrity? celebrity222 = repository.getCelebrityById(222);
        if (celebrity222 != null) {
            Console.WriteLine($"Id = {celebrity222.id}, Firstname = {celebrity222.Firstname}, " + $"Surname = {celebrity222.surname}, PhotoPath = {celebrity222.PhotoPath} ");
        }
        else {
            Console.WriteLine("Not Found 222");
        }
        
        foreach (Celebrity celebrity in repository.getCelebritiesBySurname("Chomsky")) {
            Console.WriteLine($"Id = {celebrity.id}, Firstname = {celebrity.Firstname}, " + $"Surname = {celebrity.surname}, PhotoPath = {celebrity.PhotoPath} ");
        }
        foreach (Celebrity celebrity in repository.getCelebritiesBySurname("Knuth")) {
            Console.WriteLine($"Id = {celebrity.id}, Firstname = {celebrity.Firstname}, " + $"Surname = {celebrity.surname}, PhotoPath = {celebrity.PhotoPath} ");
        }
        foreach (Celebrity celebrity in repository.getCelebritiesBySurname("XXXX")) {
            Console.WriteLine($"Id = {celebrity.id}, Firstname = {celebrity.Firstname}, " + $"Surname = {celebrity.surname}, PhotoPath = {celebrity.PhotoPath} ");
        }
        Console.WriteLine($"PhotoPathById = {repository.getPhotoPathById(4)}");
        Console.WriteLine($"PhotoPathById = {repository.getPhotoPathById(6)}");
        Console.WriteLine($"PhotoPathById = {repository.getPhotoPathById(222)}");
    }
}
```

![](media/image6.png){width="7.263888888888889in"
height="2.915277777777778in"}

![](media/image7.png){width="7.263888888888889in"
height="4.763888888888889in"}

![](media/image8.png){width="7.263888888888889in"
height="2.5756944444444443in"}

9.  Выполните тест **Test**\_**DAL003** и убедитесь в его успешном
    выполнении.

**[Задание 3.]{.underline}**

10. Разработайте ASPA, применив следующий шаблон.

Имя решения: **ASPA**

Имя проекта: **ASPA003**

Версия .NET: **7.0** или **8.0**

![](media/image9.png){width="5.517243000874891in"
height="1.1509426946631671in"}

11. Подключите библиотеку **DAL003** к приложению **ASPA003**

12. Фрагмент кода **ASPA003** приведён ниже, исследуйте его и включите в
    своё приложение.

```cs
Repository.JSONFileName= "Celebrities.json";
using (IRepository repository = Repository.Create("Celebrities"))
{
    app.MapGet("/Celebrities", () => repository.getAllCelebrities());
    app.MapGet("/Celebrities/{id:int}", (int id) => repository.getCelebrityById(id));
    app.MapGet("/Celebrities/BySurname/{surname}", (string surname) => repository.getCelebritiesBySurname(surname));
    app.MapGet("/", () => "Hello world!");
    app.Run();
}
```

13. Проверьте следующие результаты работы.

![](media/image11.png){width="5.143345363079615in"
height="4.523611111111111in"}

![](media/image12.png){width="4.03125in" height="2.0985695538057745in"}

![](media/image13.png){width="4.067648731408574in"
height="2.2552865266841646in"}

![](media/image14.png){width="4.091106736657918in"
height="1.4901224846894139in"}

![](media/image15.png){width="4.053793744531934in"
height="2.754166666666667in"}

![](media/image16.png){width="3.99375in" height="2.3970767716535435in"}

![](media/image17.png){width="4.03886811023622in"
height="1.5658114610673666in"}

![](media/image18.png){width="4.038194444444445in"
height="1.5283114610673665in"}

![](media/image19.png){width="4.083643919510061in"
height="1.4236111111111112in"}

♣**[Задание 4.]{.underline}**

14. Внесите изменения в настройки **ASPA003 (UseStaticFiles,
    StaticFileOptions)**, таким образом, чтобы файлы фотографий были
    доступны как статические ресурсы.

15. Проверьте следующие результаты работы

![](media/image20.png){width="5.404539588801399in"
height="1.3443350831146106in"}

![](media/image21.png){width="5.514925634295713in"
height="3.0153816710411196in"}

![](media/image22.png){width="5.552238626421698in"
height="1.6493055555555556in"}

![](media/image23.png){width="5.496468722659667in"
height="3.5373140857392826in"}

♣**[Задание 5.]{.underline}**

16. Внесите изменения в настройки **ASPA003 (UseDirectoryBrowser,
    DirectoryBrowserOptions)**, таким образом, чтобы файлы фотографий
    были доступны для отображения.

17. Проверьте следующие результаты работы

(на картинке окно браузера по пути `localhost:5153/Celebrities/download/`, отображается таблица Name, Size, Last Modified из папки Celebrities)

![](media/image24.png){width="3.8373753280839895in"
height="3.36757874015748in"}

![](media/image25.png){width="3.807525153105862in"
height="2.8849562554680666in"}

18. Внесите изменения в настройки **ASPA003 (UseStaticFiles,
    StaticFileOptions, OnPrepareResponse)**, таким образом, чтобы файлы
    фотографий были доступны скачивания.

19. Проверьте следующие результаты работы

![](media/image26.png){width="3.7254352580927383in"
height="2.7930325896762906in"}

![](media/image27.png){width="5.031405293088364in"
height="2.3270734908136483in"}

**[Задание 6.]{.underline}**Ответьте на вопросы

20. Поясните назначение паттерна **Repository**.

21. Для чего в паттерне **Repository** применяется интерфейс
    **IRepository**?

22. Для чего применяется интерфейс IDisposable?

23. Поясните работу следующей конструкции языка С#
    ![](media/image28.emf){width="4.589583333333334in"
    height="0.41805555555555557in"}

24. Поясните смысл слов: сериализация и десерилизация.

25. Расшифруйте аббревиатуру JSON, поясните смысл.

26. ♣ Кто такой Эдсгер Дейкстра?☺
