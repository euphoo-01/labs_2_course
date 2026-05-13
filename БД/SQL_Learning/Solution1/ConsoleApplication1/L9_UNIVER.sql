DECLARE
    @c char(10) = 'SQL',
    @vc varchar(30) = 'Server',
    @dt datetime,
    @tm time,
    @i int,
    @si smallint,
    @ti tinyint,
    @n numeric(12, 5);

SET @dt = GETDATE();
SET @tm = CONVERT(time, SYSDATETIME());
SET @i = 2026;

SELECT @si = 9,
       @ti = 8,
       @n = CAST(12345.67891 AS numeric(12, 5));

SELECT @c  AS [char],
       @vc AS [varchar],
       @dt AS [datetime],
       @tm AS [time];

PRINT 'int          : ' + CAST(@i AS varchar(20));
PRINT 'smallint     : ' + CAST(@si AS varchar(20));
PRINT 'tinyint      : ' + CAST(@ti AS varchar(20));
PRINT 'numeric(12,5): ' + CAST(@n AS varchar(30));
GO



DECLARE @totalCapacity int,
    @avgCapacity numeric(10, 2),
    @audCount int,
    @lessAvgCount int,
    @percentLess numeric(10, 2);

SELECT @totalCapacity = SUM(AUDITORIUM_CAPACITY),
       @avgCapacity = AVG(CAST(AUDITORIUM_CAPACITY AS numeric(10, 2))),
       @audCount = COUNT(*)
FROM dbo.AUDITORIUM;

IF @totalCapacity > 200
    BEGIN
        SELECT @lessAvgCount = COUNT(*)
        FROM dbo.AUDITORIUM
        WHERE AUDITORIUM_CAPACITY < @avgCapacity;

        SET @percentLess = CAST(@lessAvgCount * 100.0 / NULLIF(@audCount, 0) AS numeric(10, 2));

        SELECT @totalCapacity AS [Общая вместимость],
               @audCount      AS [Количество аудиторий],
               @avgCapacity   AS [Средняя вместимость],
               @lessAvgCount  AS [Аудиторий меньше средней вместимости],
               @percentLess   AS [Процент таких аудиторий];
    END
ELSE
    BEGIN
        PRINT N'Общая вместимость аудиторий: ' + CAST(ISNULL(@totalCapacity, 0) AS varchar(20));
    END;
GO



SELECT TOP (5) *
FROM dbo.AUDITORIUM;
PRINT '@@ROWCOUNT   : ' + CAST(@@ROWCOUNT AS varchar(20));
PRINT '@@VERSION    : ' + CAST(@@VERSION AS varchar(4000));
PRINT '@@SPID       : ' + CAST(@@SPID AS varchar(20));
PRINT '@@ERROR      : ' + CAST(@@ERROR AS varchar(20));
PRINT '@@SERVERNAME : ' + CAST(@@SERVERNAME AS varchar(128));
PRINT '@@TRANCOUNT  : ' + CAST(@@TRANCOUNT AS varchar(20));
PRINT '@@FETCH_STATUS: ' + CAST(@@FETCH_STATUS AS varchar(20));
PRINT '@@NESTLEVEL  : ' + CAST(@@NESTLEVEL AS varchar(20));

PRINT N'Округление             : ' + CAST(ROUND(12345.12345, 2) AS varchar(20));
PRINT N'Нижнее целое           : ' + CAST(FLOOR(24.5) AS varchar(20));
PRINT N'Возведение в степень   : ' + CAST(POWER(12.0, 2) AS varchar(20));
PRINT N'Логарифм               : ' + CAST(LOG(144.0) AS varchar(20));
PRINT N'Корень квадратный      : ' + CAST(SQRT(144.0) AS varchar(20));
PRINT N'Экспонента             : ' + CAST(EXP(4.96981) AS varchar(20));
PRINT N'Абсолютное значение    : ' + CAST(ABS(-5) AS varchar(20));
PRINT N'Синус                  : ' + CAST(SIN(PI()) AS varchar(30));
PRINT N'Подстрока              : ' + SUBSTRING('1234567890', 3, 2);
PRINT N'Удалить пробелы справа : ' + RTRIM('12345     ') + 'X';
PRINT N'Удалить пробелы слева  : ' + 'X' + LTRIM('     67890');
PRINT N'Нижний регистр         : ' + LOWER(N'ВЕРХНИЙ РЕГИСТР');
PRINT N'Верхний регистр        : ' + UPPER(N'нижний регистр');
PRINT N'Заменить               : ' + REPLACE('1234512345', '5', 'X');
PRINT N'Строка пробелов        : ' + 'X' + SPACE(5) + 'X';
PRINT N'Повторить строку       : ' + REPLICATE('12', 5);
PRINT N'Найти по шаблону       : ' + CAST(PATINDEX('%Y_Y%', '123456YxY7890') AS varchar(5));

DECLARE @t time(7) = SYSDATETIME(), @dateNow datetime = GETDATE();
PRINT N'Текущее время          : ' + CONVERT(varchar(16), @t);
PRINT N'Текущая дата           : ' + CONVERT(varchar(12), @dateNow, 103);
PRINT N'+1 день                : ' + CONVERT(varchar(12), DATEADD(day, 1, @dateNow), 103);
GO



DECLARE @calc TABLE
              (
                  x numeric(12, 4),
                  y numeric(12, 4),
                  z numeric(18, 6)
              );

INSERT INTO @calc (x, y, z)
VALUES (1.0000, 2.0000, CAST((POWER(1.0000, 2) + POWER(2.0000, 2)) / NULLIF(1.0000 + 2.0000, 0) AS numeric(18, 6))),
       (5.5000, 3.2000, CAST((POWER(5.5000, 2) + POWER(3.2000, 2)) / NULLIF(5.5000 + 3.2000, 0) AS numeric(18, 6))),
       (-2.0000, 7.0000, CAST((POWER(-2.0000, 2) + POWER(7.0000, 2)) / NULLIF(-2.0000 + 7.0000, 0) AS numeric(18, 6)));

SELECT x, y, z
FROM @calc;



DECLARE @fio nvarchar(100), @p1 int, @p2 int;

SELECT TOP (1) @fio = NAME
FROM dbo.STUDENT
WHERE NAME IS NOT NULL
  AND LEN(NAME) - LEN(REPLACE(NAME, N' ', N'')) >= 2
ORDER BY IDSTUDENT;

SET @p1 = CHARINDEX(N' ', @fio);
SET @p2 = CHARINDEX(N' ', @fio, @p1 + 1);

SELECT @fio    AS [Полное ФИО],
       CASE
           WHEN @fio IS NULL OR @p1 = 0 OR @p2 = 0 THEN @fio
           ELSE LEFT(@fio, @p1 - 1) + N' ' + SUBSTRING(@fio, @p1 + 1, 1) + N'. ' + SUBSTRING(@fio, @p2 + 1, 1) + N'.'
           END AS [Сокращенное ФИО];



SELECT IDSTUDENT,
       NAME,
       BDAY,
       DATEDIFF(year, BDAY, GETDATE())
           - CASE
                 WHEN DATEADD(year, DATEDIFF(year, BDAY, GETDATE()), BDAY) > CAST(GETDATE() AS date) THEN 1
                 ELSE 0 END AS [Возраст]
FROM dbo.STUDENT
WHERE BDAY IS NOT NULL
  AND MONTH(BDAY) = MONTH(DATEADD(month, 1, GETDATE()));



DECLARE @groupId int;

SELECT TOP (1) @groupId = S.IDGROUP
FROM dbo.STUDENT AS S
         JOIN dbo.PROGRESS AS P ON P.IDSTUDENT = S.IDSTUDENT
ORDER BY S.IDGROUP;

SELECT S.IDGROUP,
       S.NAME,
       P.SUBJECT,
       P.PDATE,
       DATENAME(weekday, P.PDATE) AS [День недели]
FROM dbo.STUDENT AS S
         JOIN dbo.PROGRESS AS P ON P.IDSTUDENT = S.IDSTUDENT
WHERE S.IDGROUP = @groupId
  AND (P.SUBJECT = N'БД' OR P.SUBJECT LIKE N'БД%')
ORDER BY P.PDATE, S.NAME;
GO



DECLARE @badMarks int;

SELECT @badMarks = COUNT(*)
FROM dbo.PROGRESS
WHERE NOTE < 4;

IF @badMarks > 0
    BEGIN
        PRINT N'В таблице PROGRESS есть неудовлетворительные оценки: ' + CAST(@badMarks AS varchar(20));

        SELECT S.NAME,
               P.SUBJECT,
               P.PDATE,
               P.NOTE
        FROM dbo.PROGRESS AS P
                 JOIN dbo.STUDENT AS S ON S.IDSTUDENT = P.IDSTUDENT
        WHERE P.NOTE < 4
        ORDER BY P.NOTE, S.NAME;
    END
ELSE
    BEGIN
        PRINT N'Неудовлетворительные оценки в таблице PROGRESS не найдены.';
    END;
GO



DECLARE @faculty char(10) = N'ИТ';

SELECT F.FACULTY_NAME AS [Факультет],
       S.NAME         AS [Студент],
       P.SUBJECT      AS [Дисциплина],
       P.NOTE         AS [Оценка],
       CASE
           WHEN P.NOTE BETWEEN 9 AND 10 THEN N'отлично'
           WHEN P.NOTE BETWEEN 7 AND 8 THEN N'хорошо'
           WHEN P.NOTE BETWEEN 4 AND 6 THEN N'удовлетворительно'
           WHEN P.NOTE BETWEEN 1 AND 3 THEN N'неудовлетворительно'
           ELSE N'нет оценки'
           END        AS [Характеристика]
FROM dbo.PROGRESS AS P
         JOIN dbo.STUDENT AS S ON S.IDSTUDENT = P.IDSTUDENT
         JOIN dbo.GROUPS AS G ON G.IDGROUP = S.IDGROUP
         JOIN dbo.FACULTY AS F ON F.FACULTY = G.FACULTY
WHERE F.FACULTY = @faculty
ORDER BY S.NAME, P.SUBJECT;
GO



IF OBJECT_ID('tempdb..#L9_UNIVER_TEMP') IS NOT NULL DROP TABLE #L9_UNIVER_TEMP;

CREATE TABLE #L9_UNIVER_TEMP
(
    RowNumber   int          NOT NULL PRIMARY KEY,
    RandomValue int          NOT NULL,
    CommentText varchar(100) NOT NULL
);

DECLARE @counter int = 1;
WHILE @counter <= 10
    BEGIN
        INSERT INTO #L9_UNIVER_TEMP (RowNumber, RandomValue, CommentText)
        VALUES (@counter, CAST(RAND(CHECKSUM(NEWID())) * 100 AS int), 'UNIVER row ' + CAST(@counter AS varchar(10)));

        SET @counter += 1;
    END;

SELECT *
FROM #L9_UNIVER_TEMP
ORDER BY RowNumber;
DROP TABLE #L9_UNIVER_TEMP;
GO



EXEC (N'
    PRINT N''RETURN: начало внутреннего пакета'';
    RETURN;
    PRINT N''RETURN: эта строка не будет напечатана'';
');
PRINT N'RETURN: внешний пакет продолжил выполнение.';
GO



BEGIN TRY
    DECLARE @a int = 10, @b int = 0, @result int;
    SET @result = @a / @b;
    SELECT @result AS [Результат];
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER()    AS [ERROR_NUMBER],
           ERROR_MESSAGE()   AS [ERROR_MESSAGE],
           ERROR_LINE()      AS [ERROR_LINE],
           ERROR_PROCEDURE() AS [ERROR_PROCEDURE],
           ERROR_SEVERITY()  AS [ERROR_SEVERITY],
           ERROR_STATE()     AS [ERROR_STATE];
END CATCH;
GO
