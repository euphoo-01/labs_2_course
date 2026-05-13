DECLARE
    @c char(10) = 'MYBASE',
    @vc varchar(30) = 'Indicators',
    @dt datetime,
    @tm time,
    @i int,
    @si smallint,
    @ti tinyint,
    @n numeric(12, 5);

SET @dt = GETDATE();
SET @tm = CONVERT(time, SYSDATETIME());
SET @i = 9;

SELECT @si = 25,
       @ti = 3,
       @n = CAST(9876.54321 AS numeric(12, 5));

SELECT @c  AS [char],
       @vc AS [varchar],
       @dt AS [datetime],
       @tm AS [time];

PRINT 'int          : ' + CAST(@i AS varchar(20));
PRINT 'smallint     : ' + CAST(@si AS varchar(20));
PRINT 'tinyint      : ' + CAST(@ti AS varchar(20));
PRINT 'numeric(12,5): ' + CAST(@n AS varchar(30));
GO



DECLARE @totalValue decimal(18, 2),
    @avgValue decimal(18, 2),
    @valueCount int,
    @lessAvgCount int,
    @percentLess decimal(10, 2);

SELECT @totalValue = SUM([Значение]),
       @avgValue = AVG(CAST([Значение] AS decimal(18, 2))),
       @valueCount = COUNT(*)
FROM dbo.[Значения показателей];

IF ISNULL(@totalValue, 0) > 200
    BEGIN
        SELECT @lessAvgCount = COUNT(*)
        FROM dbo.[Значения показателей]
        WHERE [Значение] < @avgValue;

        SET @percentLess = CAST(@lessAvgCount * 100.0 / NULLIF(@valueCount, 0) AS decimal(10, 2));

        SELECT @totalValue   AS [Общая сумма значений],
               @valueCount   AS [Количество значений],
               @avgValue     AS [Среднее значение],
               @lessAvgCount AS [Значений меньше среднего],
               @percentLess  AS [Процент таких значений];
    END
ELSE
    BEGIN
        PRINT N'Общая сумма значений показателей: ' + CAST(ISNULL(@totalValue, 0) AS varchar(30));
    END;
GO



SELECT TOP (5) *
FROM dbo.[Значения показателей];
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
VALUES (2.0000, 4.0000, CAST((POWER(2.0000, 2) + POWER(4.0000, 2)) / NULLIF(2.0000 + 4.0000, 0) AS numeric(18, 6))),
       (8.5000, 1.5000, CAST((POWER(8.5000, 2) + POWER(1.5000, 2)) / NULLIF(8.5000 + 1.5000, 0) AS numeric(18, 6))),
       (-3.0000, 9.0000, CAST((POWER(-3.0000, 2) + POWER(9.0000, 2)) / NULLIF(-3.0000 + 9.0000, 0) AS numeric(18, 6)));

SELECT x, y, z
FROM @calc;



DECLARE @fio nvarchar(100), @p1 int, @p2 int;

SELECT TOP (1) @fio = [Контактное лицо]
FROM dbo.[Предприятия]
WHERE [Контактное лицо] IS NOT NULL
  AND LEN([Контактное лицо]) - LEN(REPLACE([Контактное лицо], N' ', N'')) >= 2
ORDER BY [Название предприятия];

SET @p1 = CHARINDEX(N' ', @fio);
SET @p2 = CHARINDEX(N' ', @fio, @p1 + 1);

SELECT @fio    AS [Полное ФИО],
       CASE
           WHEN @fio IS NULL OR @p1 = 0 OR @p2 = 0 THEN @fio
           ELSE LEFT(@fio, @p1 - 1) + N' ' + SUBSTRING(@fio, @p1 + 1, 1) + N'. ' + SUBSTRING(@fio, @p2 + 1, 1) + N'.'
           END AS [Сокращенное ФИО];



SELECT P.[Название предприятия],
       TP.[Название показателя],
       ZP.[Значение],
       ZP.[Дата]
FROM dbo.[Значения показателей] AS ZP
         JOIN dbo.[Предприятия] AS P ON P.[ID предприятия] = ZP.[ID предприятия]
         JOIN dbo.[Типы показателей] AS TP ON TP.[ID типа] = ZP.[ID типа показателя]
WHERE ZP.[Дата] IS NOT NULL
  AND MONTH(ZP.[Дата]) = MONTH(DATEADD(month, 1, GETDATE()))
ORDER BY ZP.[Дата], P.[Название предприятия];



DECLARE @enterpriseId uniqueidentifier;

SELECT TOP (1) @enterpriseId = [ID предприятия]
FROM dbo.[Значения показателей]
ORDER BY [Дата];

SELECT P.[Название предприятия],
       TP.[Название показателя],
       ZP.[Значение],
       ZP.[Дата],
       DATENAME(weekday, ZP.[Дата]) AS [День недели]
FROM dbo.[Значения показателей] AS ZP
         JOIN dbo.[Предприятия] AS P ON P.[ID предприятия] = ZP.[ID предприятия]
         JOIN dbo.[Типы показателей] AS TP ON TP.[ID типа] = ZP.[ID типа показателя]
WHERE ZP.[ID предприятия] = @enterpriseId
ORDER BY ZP.[Дата];
GO



DECLARE @criticalCount int;

SELECT @criticalCount = COUNT(*)
FROM dbo.[Значения показателей] AS ZP
         JOIN dbo.[Типы показателей] AS TP ON TP.[ID типа] = ZP.[ID типа показателя]
WHERE TP.[Важность] IN (N'Высокая', N'Критичная', N'важно')
   OR ZP.[Значение] > 1000;

IF @criticalCount > 0
    BEGIN
        PRINT N'Найдены важные или крупные значения показателей: ' + CAST(@criticalCount AS varchar(20));

        SELECT P.[Название предприятия],
               TP.[Название показателя],
               TP.[Важность],
               ZP.[Значение],
               ZP.[Дата]
        FROM dbo.[Значения показателей] AS ZP
                 JOIN dbo.[Предприятия] AS P ON P.[ID предприятия] = ZP.[ID предприятия]
                 JOIN dbo.[Типы показателей] AS TP ON TP.[ID типа] = ZP.[ID типа показателя]
        WHERE TP.[Важность] IN (N'Высокая', N'Критичная', N'важно')
           OR ZP.[Значение] > 1000
        ORDER BY ZP.[Значение] DESC;
    END
ELSE
    BEGIN
        PRINT N'Важные или крупные значения показателей не найдены.';
    END;
GO



SELECT P.[Название предприятия],
       TP.[Название показателя],
       TP.[Важность],
       ZP.[Значение],
       CASE
           WHEN ZP.[Значение] >= 1000 THEN N'крупное значение'
           WHEN ZP.[Значение] >= 500 THEN N'среднее значение'
           WHEN ZP.[Значение] > 0 THEN N'малое значение'
           WHEN ZP.[Значение] = 0 THEN N'нулевое значение'
           ELSE N'отрицательное значение'
           END AS [Категория значения],
       CASE
           WHEN TP.[Важность] IN (N'Высокая', N'Критичная') THEN N'требует приоритетного контроля'
           WHEN TP.[Важность] IN (N'Средняя') THEN N'стандартный контроль'
           ELSE N'базовый контроль'
           END AS [Контроль]
FROM dbo.[Значения показателей] AS ZP
         JOIN dbo.[Предприятия] AS P ON P.[ID предприятия] = ZP.[ID предприятия]
         JOIN dbo.[Типы показателей] AS TP ON TP.[ID типа] = ZP.[ID типа показателя]
ORDER BY P.[Название предприятия], TP.[Название показателя], ZP.[Дата];
GO



IF OBJECT_ID('tempdb..#L9_MYBASE_TEMP') IS NOT NULL DROP TABLE #L9_MYBASE_TEMP;

CREATE TABLE #L9_MYBASE_TEMP
(
    RowNumber   int          NOT NULL PRIMARY KEY,
    RandomValue int          NOT NULL,
    CommentText varchar(100) NOT NULL
);

DECLARE @counter int = 1;
WHILE @counter <= 10
    BEGIN
        INSERT INTO #L9_MYBASE_TEMP (RowNumber, RandomValue, CommentText)
        VALUES (@counter, CAST(RAND(CHECKSUM(NEWID())) * 100 AS int), 'X_MyBase row ' + CAST(@counter AS varchar(10)));

        SET @counter += 1;
    END;

SELECT *
FROM #L9_MYBASE_TEMP
ORDER BY RowNumber;
DROP TABLE #L9_MYBASE_TEMP;
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
