# Ответы на вопросы (ЛР №15 "Платформа параллельных вычислений")

## 1. Что такое TPL? Как и для чего используется тип Task

TPL (Task Parallel Library) — встроенная в .NET библиотека для организации параллельных и асинхронных вычислений.

Task — основной тип в TPL, представляющий единицу асинхронной/параллельной работы.

Для чего используется:
- Распараллеливание вычислений на несколько ядер
- Асинхронное выполнение операций (особенно I/O)
- Упрощённое управление потоками по сравнению с Thread

---

## 2. Почему эффект от распараллеливания наблюдается на большом количестве элементов?

Причины:
- Накладные расходы на планирование имеют фиксированные затраты. На малых объёмах эти затраты "съедают" выигрыш от параллелизма
- На малых объёмах потоки просто успевают переключаться и синхронизироваться, что медленнее чем последовательное выполнение
- Параллелизм выгоден, когда каждый поток берёт достаточно "жирный" кусок вычислений (миллионы операций), а не несколько операций

На миллионах элементов эффект позитивный, потому что вычисления "перекрывают" накладные расходы.

---

## 3. В чем основные достоинства работы с задачами по сравнению с потоками?

Достоинства Task vs Thread:

1. Меньше ресурсов — Task переиспользуется (thread pool), потоки требуют больше памяти
2. Проще синтаксис — Task.Run() vs создание new Thread()
3. Встроенная поддержка async/await — Task работает с async/await, потоки нет
4. Лучшее планирование — пул потоков работает с Task через work-stealing очередь
5. Композиция — легко комбинировать задачи (Task.WaitAll, Task.WhenAll, ContinueWith)
6. Отмена — встроенная поддержка CancellationToken
7. Обработка исключений — исключения в Task "оборачиваются" в AggregateException

---

## 4. Приведите три способа создания и/или запуска Task?

Способ 1: Task.Run()
var task = Task.Run(() => DoWork());
task.Wait();

text

Способ 2: new Task() + Start()
var task = new Task(() => DoWork());
task.Start();
task.Wait();

text

Способ 3: Task.Factory.StartNew()
var task = Task.Factory.StartNew(() => DoWork());
task.Wait();

text

---

## 5. Как и для чего используют методы Wait(), WaitAll() и WaitAny()?

Wait() — ждёт завершения одной задачи. Блокирует текущий поток до завершения.

WaitAll() — ждёт завершения всех задач одновременно.

WaitAny() — ждёт завершения любой из задач. Возвращает индекс завершённой первой.

---

## 6. Приведите пример синхронного запуска Task?

Синхронный запуск означает запустить задачу и дождаться её выполнения:

var task = Task.Run(() => {
Console.WriteLine("Работаю...");
Thread.Sleep(1000);
return 42;
});

task.Wait();
int result = task.Result;
Console.WriteLine(result);

text

---

## 7. Как создать задачу с возвратом результата?

Task<T> — задача, которая возвращает результат типа T:

var task = Task.Run<int>(() => {
Thread.Sleep(500);
return 42;
});

int result = task.Result;

text

Или через async:

public async Task<int> GetDataAsync() {
await Task.Delay(500);
return 42;
}

int result = await GetDataAsync();

text

---

## 8. Как обработать исключение, если оно произошло при выполнении Task?

Способ 1: try/catch вокруг Wait()

var task = Task.Run(() => {
throw new InvalidOperationException("Ошибка");
});

try {
task.Wait();
} catch (AggregateException ex) {
Console.WriteLine(ex.InnerException?.Message);
}

text

Способ 2: async/await (рекомендуется)

try {
await Task.Run(() => throw new Exception("Error"));
} catch (Exception ex) {
Console.WriteLine(ex.Message);
}

text

---

## 9. Что такое CancellationToken и как с его помощью отменить выполнение задач?

CancellationToken — механизм для корректного прерывания асинхронной работы. Не "убивает" поток, а даёт сигнал задаче на отмену.

var cts = new CancellationTokenSource();
var token = cts.Token;

var task = Task.Run(() => {
for (int i = 0; i < 1000; i++) {
token.ThrowIfCancellationRequested();
DoSomeWork();
Thread.Sleep(100);
}
});

Thread.Sleep(500);
cts.Cancel();

try {
task.Wait();
} catch (AggregateException ex) {
if (ex.InnerException is OperationCanceledException)
Console.WriteLine("Задача отменена");
}

text

---

## 10. Как организовать задачу продолжения (continuation task)?

Continuation task — задача, которая начинается после завершения другой задачи.

var task = Task.Run(() => 42)
.ContinueWith(prev => {
int result = prev.Result;
return result * 2;
});

task.Wait();
Console.WriteLine(task.Result);

text

---

## 11. Как и для чего используется объект ожидания при создании задач продолжения?

Объект ожидания (awaiter) используется для создания задач продолжения:

var task = Task.Run(() => {
Thread.Sleep(500);
return "Результат";
});

var awaiter = task.GetAwaiter();

if (awaiter.IsCompleted) {
string result = awaiter.GetResult();
Console.WriteLine(result);
} else {
awaiter.OnCompleted(() => {
string result = awaiter.GetResult();
Console.WriteLine(result);
});
}

text

Awaiter позволяет более гибкое управление продолжениями и является основой для async/await.

---

## 12. Поясните назначение класса System.Threading.Tasks.Parallel?

Parallel — статический класс с методами для параллельного выполнения циклов:

- Parallel.For(...) — параллельный цикл for
- Parallel.ForEach(...) — параллельный цикл foreach
- Parallel.Invoke(...) — параллельное выполнение операций

Назначение:
- Распараллелить обработку больших объёмов данных
- Упростить параллелизм по сравнению с ручным созданием задач
- Автоматическое распределение работы между ядрами

---

## 13. Приведите пример задачи с Parallel.For(int, int, Action<int>)

int[] arr = new int;

Parallel.For(0, arr.Length, i => {
arr[i] = i * 2;
});

Console.WriteLine("Завершено");

text

Пример с опциями:

var options = new ParallelOptions {
MaxDegreeOfParallelism = Environment.ProcessorCount
};

Parallel.For(0, arr.Length, options, i => {
arr[i] = i * 3 + 1;
});

text

---

## 14. Приведите пример задачи с Parallel.ForEach

var numbers = Enumerable.Range(1, 1000000).ToList();

Parallel.ForEach(numbers, num => {
ProcessNumber(num);
});

void ProcessNumber(int num) {
int result = num * num;
}

text

Пример с ранней остановкой:

Parallel.ForEach(numbers, (num, loopState) => {
if (num > 500)
loopState.Stop();
else
Console.WriteLine(num);
});

text

---

## 15. Приведите пример с Parallel.Invoke()

Parallel.Invoke(
() => Console.WriteLine("Операция 1"),
() => Console.WriteLine("Операция 2"),
() => Console.WriteLine("Операция 3"),
() => Console.WriteLine("Операция 4")
);

Console.WriteLine("Все операции завершены");

text

Пример с параметрами:

int[] results = new int;​

Parallel.Invoke(
() => results = Calculate(10),
() => results = Calculate(20),​
() => results = Calculate(30)​
);

int Calculate(int x) => x * x;

text

---

## 16. Как с использованием CancellationToken отменить параллельные операции?

var cts = new CancellationTokenSource();
var token = cts.Token;

try {
Parallel.For(0, 1000000,
new ParallelOptions { CancellationToken = token },
i => {
token.ThrowIfCancellationRequested();
DoSomeWork(i);
}
);
} catch (OperationCanceledException) {
Console.WriteLine("Цикл отменён");
}

Thread.Sleep(500);
cts.Cancel();

text

---

## 17. Для чего используют BlockingCollection<T>, в чем ее особенность?

BlockingCollection<T> — потокобезопасная коллекция для передачи данных между потоками (Producer-Consumer паттерн).

Особенности:
- Потокобезопасна
- Может быть ограничена по размеру (boundedCapacity)
- Блокирующие операции: Take() ждёт элемента, Add() может ждать освобождения буфера
- CompleteAdding() сигнализирует об окончании

var queue = new BlockingCollection<int>(boundedCapacity: 10);

var producer = Task.Run(() => {
for (int i = 0; i < 100; i++) {
queue.Add(i);
Thread.Sleep(100);
}
queue.CompleteAdding();
});

var consumer = Task.Run(() => {
while (queue.TryTake(out int item)) {
Console.WriteLine(item);
Thread.Sleep(150);
}
});

Task.WaitAll(producer, consumer);

text

---

## 18. Как используя async и await организовать асинхронное выполнение метода?

async/await — синтаксический сахар для работы с асинхронными операциями.

Правила:
- Метод помечается async
- Внутри используется await для ожидания асинхронных операций
- Метод должен возвращать Task или Task<T>

Пример 1: Базовый async метод

public async Task<string> FetchDataAsync() {
await Task.Delay(1000);
return "Данные получены";
}

string result = await FetchDataAsync();
Console.WriteLine(result);

text

Пример 2: Несколько await операций

public async Task<int> ProcessDataAsync() {
int step1 = await GetDataAsync();
int step2 = await TransformAsync(step1);
int step3 = await SaveAsync(step2);
return step3;
}

text

Пример 3: Параллельные async операции

public async Task<string> FetchMultipleAsync() {
var t1 = FetchFromApiAsync("url1");
var t2 = FetchFromApiAsync("url2");
var t3 = FetchFromApiAsync("url3");

text
var results = await Task.WhenAll(t1, t2, t3);
return $"Получено {results.Length} ответов";
}

text

Пример 4: Обработка исключений в async

public async Task<string> SafeOperationAsync() {
try {
return await SomeFailingOperationAsync();
} catch (Exception ex) {
Console.WriteLine($"Ошибка: {ex.Message}");
return "Ошибка обработана";
}
}

---