/*
    Лабораторная работа №10. Создание и применение индексов
    База данных: X_MyBase

    Структура X_MyBase по предоставленному скриншоту:
      dbo.[Предприятия]
      dbo.[Типы показателей]
      dbo.[Значения показателей]

    Перед выполнением запросов для анализа стоимости в SSMS желательно включить:
      - Include Actual Execution Plan / Display Estimated Execution Plan;
      - SET STATISTICS IO, TIME ON.
*/
USE [X_MyBase];
GO

SET NOCOUNT ON;
SET STATISTICS IO ON;
SET STATISTICS TIME ON;
GO

/* ============================================================
   Задание 7. Создать необходимые индексы и проанализировать планы
   запросов с использованием этих индексов для таблиц X_MyBase.
   ============================================================ */

PRINT N'Исходный список индексов X_MyBase';
SELECT
    OBJECT_SCHEMA_NAME(t.object_id) AS [Схема],
    t.name                          AS [Таблица],
    i.name                          AS [Индекс],
    i.type_desc                     AS [Тип индекса],
    i.is_unique                     AS [Уникальный],
    i.is_primary_key                AS [Первичный ключ]
FROM sys.tables AS t
    INNER JOIN sys.indexes AS i ON i.object_id = t.object_id
WHERE i.index_id > 0
ORDER BY [Схема], [Таблица], [Индекс];
GO

/* Очистка индексов лабораторной при повторном запуске */
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_L10_Values_Enterprise_Date' AND object_id = OBJECT_ID(N'dbo.[Значения показателей]'))
    DROP INDEX IX_L10_Values_Enterprise_Date ON dbo.[Значения показателей];
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_L10_Values_Type_Date' AND object_id = OBJECT_ID(N'dbo.[Значения показателей]'))
    DROP INDEX IX_L10_Values_Type_Date ON dbo.[Значения показателей];
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_L10_Values_Date_Value' AND object_id = OBJECT_ID(N'dbo.[Значения показателей]'))
    DROP INDEX IX_L10_Values_Date_Value ON dbo.[Значения показателей];
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_L10_Enterprise_Name' AND object_id = OBJECT_ID(N'dbo.[Предприятия]'))
    DROP INDEX IX_L10_Enterprise_Name ON dbo.[Предприятия];
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_L10_Type_Importance_Name' AND object_id = OBJECT_ID(N'dbo.[Типы показателей]'))
    DROP INDEX IX_L10_Type_Importance_Name ON dbo.[Типы показателей];
GO
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_L10_Type_HighImportance' AND object_id = OBJECT_ID(N'dbo.[Типы показателей]'))
    DROP INDEX IX_L10_Type_HighImportance ON dbo.[Типы показателей];
GO

/* ------------------------------------------------------------
   7.1. Запросы до создания дополнительных индексов.
   ------------------------------------------------------------ */
PRINT N'Задание 7.1. Запросы до создания дополнительных индексов';

SELECT
    p.[Название предприятия],
    v.[Дата],
    v.[Значение]
FROM dbo.[Значения показателей] AS v
    INNER JOIN dbo.[Предприятия] AS p
        ON p.[ID предприятия] = v.[ID предприятия]
WHERE p.[Название предприятия] LIKE N'А%'
ORDER BY p.[Название предприятия], v.[Дата];
GO

SELECT
    t.[Название показателя],
    t.[Важность],
    v.[Дата],
    v.[Значение]
FROM dbo.[Значения показателей] AS v
    INNER JOIN dbo.[Типы показателей] AS t
        ON t.[ID типа] = v.[ID типа показателя]
WHERE t.[Важность] IN (N'Высокая', N'Важно', N'Важный')
ORDER BY t.[Название показателя], v.[Дата];
GO

SELECT
    v.[Дата],
    v.[Значение],
    v.[ID предприятия],
    v.[ID типа показателя]
FROM dbo.[Значения показателей] AS v
WHERE v.[Дата] >= DATEADD(day, -365, CONVERT(date, GETDATE()))
  AND v.[Значение] >= 100
ORDER BY v.[Дата], v.[Значение];
GO

/* ------------------------------------------------------------
   7.2. Создание индексов.
   ------------------------------------------------------------ */
PRINT N'Задание 7.2. Создание индексов';

/* Индекс для поиска предприятия по названию и вывода контактных данных. */
CREATE NONCLUSTERED INDEX IX_L10_Enterprise_Name
ON dbo.[Предприятия] ([Название предприятия])
INCLUDE ([Телефон], [Контактное лицо], [Расчетный счет], [БИК банка]);
GO

/* Составной индекс для выборок значений конкретного предприятия по датам. */
CREATE NONCLUSTERED INDEX IX_L10_Values_Enterprise_Date
ON dbo.[Значения показателей] ([ID предприятия], [Дата])
INCLUDE ([ID типа показателя], [Значение]);
GO

/* Составной индекс для выборок значений конкретного типа показателя по датам. */
CREATE NONCLUSTERED INDEX IX_L10_Values_Type_Date
ON dbo.[Значения показателей] ([ID типа показателя], [Дата])
INCLUDE ([ID предприятия], [Значение]);
GO

/* Индекс покрытия для фильтрации и сортировки по дате и значению. */
CREATE NONCLUSTERED INDEX IX_L10_Values_Date_Value
ON dbo.[Значения показателей] ([Дата], [Значение])
INCLUDE ([ID предприятия], [ID типа показателя]);
GO

/* Составной индекс для отбора типов показателей по важности и названию. */
CREATE NONCLUSTERED INDEX IX_L10_Type_Importance_Name
ON dbo.[Типы показателей] ([Важность], [Название показателя])
INCLUDE ([ID типа]);
GO

/* Фильтруемый индекс только для важных типов показателей. */
CREATE NONCLUSTERED INDEX IX_L10_Type_HighImportance
ON dbo.[Типы показателей] ([Название показателя])
INCLUDE ([ID типа], [Важность])
WHERE [Важность] IN (N'Высокая', N'Важно', N'Важный');
GO

PRINT N'Список индексов после создания индексов лабораторной';
SELECT
    OBJECT_SCHEMA_NAME(t.object_id) AS [Схема],
    t.name                          AS [Таблица],
    i.name                          AS [Индекс],
    i.type_desc                     AS [Тип индекса],
    i.is_unique                     AS [Уникальный],
    i.has_filter                    AS [Фильтруемый],
    i.filter_definition             AS [Условие фильтра]
FROM sys.tables AS t
    INNER JOIN sys.indexes AS i ON i.object_id = t.object_id
WHERE i.name LIKE N'IX_L10_%'
ORDER BY [Схема], [Таблица], [Индекс];
GO

/* ------------------------------------------------------------
   7.3. Те же запросы после создания индексов.
   В плане выполнения должны применяться Index Seek/Index Scan по IX_L10_*.
   ------------------------------------------------------------ */
PRINT N'Задание 7.3. Запросы после создания дополнительных индексов';

SELECT
    p.[Название предприятия],
    v.[Дата],
    v.[Значение]
FROM dbo.[Значения показателей] AS v
    INNER JOIN dbo.[Предприятия] AS p
        ON p.[ID предприятия] = v.[ID предприятия]
WHERE p.[Название предприятия] LIKE N'А%'
ORDER BY p.[Название предприятия], v.[Дата];
GO

SELECT
    t.[Название показателя],
    t.[Важность],
    v.[Дата],
    v.[Значение]
FROM dbo.[Значения показателей] AS v
    INNER JOIN dbo.[Типы показателей] AS t
        ON t.[ID типа] = v.[ID типа показателя]
WHERE t.[Важность] IN (N'Высокая', N'Важно', N'Важный')
ORDER BY t.[Название показателя], v.[Дата];
GO

SELECT
    v.[Дата],
    v.[Значение],
    v.[ID предприятия],
    v.[ID типа показателя]
FROM dbo.[Значения показателей] AS v
WHERE v.[Дата] >= DATEADD(day, -365, CONVERT(date, GETDATE()))
  AND v.[Значение] >= 100
ORDER BY v.[Дата], v.[Значение];
GO

/* ------------------------------------------------------------
   7.4. Отчет по использованию индексов и фрагментации.
   ------------------------------------------------------------ */
PRINT N'Задание 7.4. Статистика использования индексов лабораторной';
SELECT
    OBJECT_NAME(i.object_id) AS [Таблица],
    i.name                   AS [Индекс],
    COALESCE(us.user_seeks, 0)   AS [User seeks],
    COALESCE(us.user_scans, 0)   AS [User scans],
    COALESCE(us.user_lookups, 0) AS [User lookups],
    COALESCE(us.user_updates, 0) AS [User updates]
FROM sys.indexes AS i
    LEFT JOIN sys.dm_db_index_usage_stats AS us
        ON us.database_id = DB_ID()
       AND us.object_id = i.object_id
       AND us.index_id = i.index_id
WHERE i.name LIKE N'IX_L10_%'
ORDER BY [Таблица], [Индекс];
GO

PRINT N'Задание 7.5. Фрагментация индексов лабораторной';
SELECT
    OBJECT_NAME(ips.object_id) AS [Таблица],
    i.name AS [Индекс],
    ips.avg_fragmentation_in_percent AS [Фрагментация (%)],
    ips.page_count AS [Количество страниц]
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, N'SAMPLED') AS ips
    INNER JOIN sys.indexes AS i
        ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE i.name LIKE N'IX_L10_%'
ORDER BY [Таблица], [Индекс];
GO

SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
GO
