IF OBJECT_ID(N'dbo.[Значения_показателей_по_датам]', N'V') IS NOT NULL DROP VIEW dbo.[Значения_показателей_по_датам];
GO
IF OBJECT_ID(N'dbo.[Типы_показателей]', N'V') IS NOT NULL DROP VIEW dbo.[Типы_показателей];
GO
IF OBJECT_ID(N'dbo.[Важные_типы_показателей]', N'V') IS NOT NULL DROP VIEW dbo.[Важные_типы_показателей];
GO
IF OBJECT_ID(N'dbo.[Показатели_предприятий]', N'V') IS NOT NULL DROP VIEW dbo.[Показатели_предприятий];
GO
IF OBJECT_ID(N'dbo.[Количество_показателей]', N'V') IS NOT NULL DROP VIEW dbo.[Количество_показателей];
GO
IF OBJECT_ID(N'dbo.[Предприятие]', N'V') IS NOT NULL DROP VIEW dbo.[Предприятие];
GO

CREATE VIEW dbo.[Предприятие]
AS
SELECT
    P.[ID предприятия]          AS [Код],
        P.[Название предприятия]    AS [Название предприятия],
        P.[Телефон]                 AS [Телефон],
        P.[Контактное лицо]         AS [Контактное лицо]
FROM dbo.[Предприятия] AS P;
GO
SELECT * FROM dbo.[Предприятие];
GO

CREATE VIEW dbo.[Количество_показателей]
AS
SELECT
    P.[Название предприятия] AS [Предприятие],
        COUNT(V.[ID значения])   AS [Количество показателей]
FROM dbo.[Предприятия] AS P
    LEFT JOIN dbo.[Значения показателей] AS V
ON V.[ID предприятия] = P.[ID предприятия]
GROUP BY P.[Название предприятия];
GO
SELECT * FROM dbo.[Количество_показателей];
GO

CREATE VIEW dbo.[Важные_типы_показателей]
AS
SELECT
    T.[ID типа]               AS [Код],
        T.[Важность]              AS [Важность],
        T.[Название показателя]   AS [Название показателя]
FROM dbo.[Типы показателей] AS T
WHERE T.[Важность] IN (N'Высокая', N'Важно', N'Важный')
   OR T.[Важность] LIKE N'Выс%'
WITH CHECK OPTION;
GO
SELECT * FROM dbo.[Важные_типы_показателей];
GO

CREATE VIEW dbo.[Показатели_предприятий]
AS
SELECT
    P.[Название предприятия]  AS [Предприятие],
        T.[Название показателя]   AS [Показатель],
        T.[Важность]              AS [Важность],
        V.[Значение]              AS [Значение],
        V.[Дата]                  AS [Дата]
FROM dbo.[Значения показателей] AS V
    INNER JOIN dbo.[Предприятия] AS P
ON P.[ID предприятия] = V.[ID предприятия]
    INNER JOIN dbo.[Типы показателей] AS T
    ON T.[ID типа] = V.[ID типа показателя];
GO
SELECT * FROM dbo.[Показатели_предприятий]
ORDER BY [Предприятие], [Показатель], [Дата];
GO

CREATE VIEW dbo.[Типы_показателей]
AS
SELECT TOP (100) PERCENT
    T.[ID типа]             AS [Код],
        T.[Название показателя] AS [Название показателя],
        T.[Важность]            AS [Важность]
FROM dbo.[Типы показателей] AS T
ORDER BY T.[Название показателя];
GO
SELECT * FROM dbo.[Типы_показателей]
ORDER BY [Название показателя];
GO

ALTER VIEW dbo.[Количество_показателей]
WITH SCHEMABINDING
AS
SELECT
    P.[Название предприятия] AS [Предприятие],
        COUNT_BIG(*)             AS [Количество показателей]
FROM dbo.[Предприятия] AS P
    INNER JOIN dbo.[Значения показателей] AS V
ON V.[ID предприятия] = P.[ID предприятия]
GROUP BY P.[Название предприятия];
GO
SELECT * FROM dbo.[Количество_показателей];
GO

SELECT
    OBJECT_SCHEMA_NAME(V.object_id) AS [Схема],
    V.name AS [Представление],
    M.is_schema_bound AS [Привязано к схеме]
FROM sys.views AS V
    INNER JOIN sys.sql_modules AS M ON M.object_id = V.object_id
WHERE V.name = N'Количество_показателей';
GO

BEGIN TRY
EXEC(N'ALTER TABLE dbo.[Значения показателей] DROP COLUMN [ID предприятия];');
END TRY
BEGIN CATCH
PRINT N'Операция с базовой таблицей заблокирована, так как представление dbo.[Количество_показателей] создано WITH SCHEMABINDING.';
    PRINT ERROR_MESSAGE();
END CATCH;
GO

CREATE VIEW dbo.[Значения_показателей_по_датам]
AS
SELECT
    V.[Дата]                  AS [Дата],
        P.[Название предприятия]  AS [Предприятие],
        T.[Название показателя]   AS [Показатель],
        V.[Значение]              AS [Значение]
FROM dbo.[Значения показателей] AS V
    INNER JOIN dbo.[Предприятия] AS P
ON P.[ID предприятия] = V.[ID предприятия]
    INNER JOIN dbo.[Типы показателей] AS T
    ON T.[ID типа] = V.[ID типа показателя];
GO
SELECT * FROM dbo.[Значения_показателей_по_датам]
ORDER BY [Дата], [Предприятие], [Показатель];
GO
