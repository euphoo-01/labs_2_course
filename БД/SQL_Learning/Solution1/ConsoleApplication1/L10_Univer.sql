/*
    Лабораторная работа №10. Создание и применение индексов
    База данных: UNIVER

    Перед выполнением запросов для анализа стоимости в SSMS желательно включить:
      - Include Actual Execution Plan / Display Estimated Execution Plan;
      - SET STATISTICS IO, TIME ON.
*/
USE [UNIVER];
GO

SET NOCOUNT ON;
SET STATISTICS IO ON;
SET STATISTICS TIME ON;
GO

/* ============================================================
   1. Определить все индексы, имеющиеся в БД UNIVER.
      Создать локальную временную таблицу, заполнить ее >= 1000 строк.
      Выполнить SELECT-запрос до и после создания кластеризованного индекса.
   ============================================================ */
PRINT N'Задание 1. Индексы базы UNIVER';

SELECT
    OBJECT_SCHEMA_NAME(t.object_id) AS [Схема],
    t.name                          AS [Таблица],
    i.name                          AS [Индекс],
    i.type_desc                     AS [Тип индекса],
    i.is_unique                     AS [Уникальный],
    i.is_primary_key                AS [Первичный ключ],
    i.is_unique_constraint          AS [Ограничение UNIQUE]
FROM sys.tables AS t
    INNER JOIN sys.indexes AS i ON i.object_id = t.object_id
WHERE i.index_id > 0
ORDER BY [Схема], [Таблица], [Индекс];
GO

IF OBJECT_ID(N'tempdb..#EXPLRE') IS NOT NULL DROP TABLE #EXPLRE;
GO

CREATE TABLE #EXPLRE
(
    TIND int NOT NULL,
    CC   int IDENTITY(1,1) NOT NULL,
    TF   varchar(100) NOT NULL
);
GO

DECLARE @i int = 0;
WHILE @i < 20000
BEGIN
    INSERT INTO #EXPLRE(TIND, TF)
    VALUES (FLOOR(30000 * RAND(CHECKSUM(NEWID()))), REPLICATE('string ', 10));
    SET @i += 1;
END;
GO

SELECT COUNT(*) AS [Количество строк в #EXPLRE] FROM #EXPLRE;
GO

PRINT N'Задание 1. Запрос до создания кластеризованного индекса';
BEGIN TRY
    CHECKPOINT;
    DBCC DROPCLEANBUFFERS WITH NO_INFOMSGS;
END TRY
BEGIN CATCH
    PRINT N'Не удалось очистить буферный кэш: ' + ERROR_MESSAGE();
END CATCH;

SELECT *
FROM #EXPLRE
WHERE TIND BETWEEN 1500 AND 2500
ORDER BY TIND;
GO

CREATE CLUSTERED INDEX IX_EXPLRE_CL
ON #EXPLRE(TIND ASC);
GO

PRINT N'Задание 1. Запрос после создания кластеризованного индекса IX_EXPLRE_CL';
BEGIN TRY
    CHECKPOINT;
    DBCC DROPCLEANBUFFERS WITH NO_INFOMSGS;
END TRY
BEGIN CATCH
    PRINT N'Не удалось очистить буферный кэш: ' + ERROR_MESSAGE();
END CATCH;

SELECT *
FROM #EXPLRE
WHERE TIND BETWEEN 1500 AND 2500
ORDER BY TIND;
GO

/* ============================================================
   2. Некластеризованный неуникальный составной индекс.
   ============================================================ */
PRINT N'Задание 2. Некластеризованный неуникальный составной индекс';

IF OBJECT_ID(N'tempdb..#EX') IS NOT NULL DROP TABLE #EX;
GO

CREATE TABLE #EX
(
    TKEY int NOT NULL,
    CC   int IDENTITY(1,1) NOT NULL,
    TF   varchar(100) NOT NULL
);
GO

DECLARE @i int = 0;
WHILE @i < 20000
BEGIN
    INSERT #EX(TKEY, TF)
    VALUES (FLOOR(30000 * RAND(CHECKSUM(NEWID()))), REPLICATE('строка ', 10));
    SET @i += 1;
END;
GO

SELECT COUNT(*) AS [Количество строк в #EX] FROM #EX;
GO

PRINT N'Задание 2. Запросы до создания составного индекса';
SELECT * FROM #EX WHERE TKEY > 1500 AND CC < 4500;
SELECT * FROM #EX ORDER BY TKEY, CC;
SELECT * FROM #EX WHERE TKEY = 556 AND CC > 3;
GO

CREATE NONCLUSTERED INDEX IX_EX_NONCLU
ON #EX(TKEY, CC);
GO

PRINT N'Задание 2. Запросы после создания составного индекса IX_EX_NONCLU';
SELECT * FROM #EX WHERE TKEY > 1500 AND CC < 4500;
SELECT * FROM #EX ORDER BY TKEY, CC;
SELECT * FROM #EX WHERE TKEY = 556 AND CC > 3;
GO

/* ============================================================
   3. Некластеризованный индекс покрытия запроса.
   ============================================================ */
PRINT N'Задание 3. Индекс покрытия запроса';

IF OBJECT_ID(N'tempdb..#EX_COVER') IS NOT NULL DROP TABLE #EX_COVER;
GO

CREATE TABLE #EX_COVER
(
    TKEY int NOT NULL,
    CC   int IDENTITY(1,1) NOT NULL,
    TF   varchar(100) NOT NULL
);
GO

DECLARE @i int = 0;
WHILE @i < 20000
BEGIN
    INSERT #EX_COVER(TKEY, TF)
    VALUES (FLOOR(30000 * RAND(CHECKSUM(NEWID()))), REPLICATE('строка ', 10));
    SET @i += 1;
END;
GO

PRINT N'Задание 3. Запрос до индекса покрытия';
SELECT CC
FROM #EX_COVER
WHERE TKEY > 15000;
GO

CREATE NONCLUSTERED INDEX IX_EX_COVER_TKEY_INCLUDE_CC
ON #EX_COVER(TKEY)
INCLUDE (CC);
GO

PRINT N'Задание 3. Запрос после индекса покрытия IX_EX_COVER_TKEY_INCLUDE_CC';
SELECT CC
FROM #EX_COVER
WHERE TKEY > 15000;
GO

/* ============================================================
   4. Некластеризованный фильтруемый индекс.
   ============================================================ */
PRINT N'Задание 4. Фильтруемый индекс';

IF OBJECT_ID(N'tempdb..#EX_FILTER') IS NOT NULL DROP TABLE #EX_FILTER;
GO

CREATE TABLE #EX_FILTER
(
    TKEY int NOT NULL,
    CC   int IDENTITY(1,1) NOT NULL,
    TF   varchar(100) NOT NULL
);
GO

DECLARE @i int = 0;
WHILE @i < 30000
BEGIN
    INSERT #EX_FILTER(TKEY, TF)
    VALUES (FLOOR(30000 * RAND(CHECKSUM(NEWID()))), REPLICATE('строка ', 10));
    SET @i += 1;
END;
GO

PRINT N'Задание 4. Запросы до фильтруемого индекса';
SELECT TKEY FROM #EX_FILTER WHERE TKEY BETWEEN 5000 AND 19999;
SELECT TKEY FROM #EX_FILTER WHERE TKEY > 15000 AND TKEY < 20000;
SELECT TKEY FROM #EX_FILTER WHERE TKEY = 17000;
GO

CREATE NONCLUSTERED INDEX IX_EX_FILTER_WHERE
ON #EX_FILTER(TKEY)
WHERE TKEY >= 15000 AND TKEY < 20000;
GO

PRINT N'Задание 4. Запросы после фильтруемого индекса IX_EX_FILTER_WHERE';
SELECT TKEY FROM #EX_FILTER WHERE TKEY BETWEEN 5000 AND 19999;
SELECT TKEY FROM #EX_FILTER WHERE TKEY > 15000 AND TKEY < 20000;
SELECT TKEY FROM #EX_FILTER WHERE TKEY = 17000;
GO

/* ============================================================
   5. Фрагментация индекса, реорганизация и перестройка.
   ============================================================ */
PRINT N'Задание 5. Фрагментация, REORGANIZE, REBUILD';

IF OBJECT_ID(N'tempdb..#EX_FRAG') IS NOT NULL DROP TABLE #EX_FRAG;
GO

CREATE TABLE #EX_FRAG
(
    TKEY int NOT NULL,
    CC   int IDENTITY(1,1) NOT NULL,
    TF   varchar(400) NOT NULL
);
GO

DECLARE @i int = 0;
WHILE @i < 20000
BEGIN
    INSERT #EX_FRAG(TKEY, TF)
    VALUES (@i, REPLICATE('fragment ', 35));
    SET @i += 1;
END;
GO

CREATE NONCLUSTERED INDEX IX_EX_FRAG_TKEY
ON #EX_FRAG(TKEY);
GO

PRINT N'Задание 5. Фрагментация после создания индекса';
SELECT
    i.name AS [Индекс],
    ips.avg_fragmentation_in_percent AS [Фрагментация (%)],
    ips.page_count AS [Количество страниц]
FROM sys.dm_db_index_physical_stats(DB_ID(N'tempdb'), OBJECT_ID(N'tempdb..#EX_FRAG'), NULL, NULL, N'SAMPLED') AS ips
    INNER JOIN tempdb.sys.indexes AS i
        ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE i.name IS NOT NULL;
GO

DECLARE @i int = 0;
WHILE @i < 20000
BEGIN
    INSERT #EX_FRAG(TKEY, TF)
    VALUES (FLOOR(40000 * RAND(CHECKSUM(NEWID()))), REPLICATE('fragment ', 35));
    SET @i += 1;
END;
GO

PRINT N'Задание 5. Фрагментация после добавления случайных строк';
SELECT
    i.name AS [Индекс],
    ips.avg_fragmentation_in_percent AS [Фрагментация (%)],
    ips.page_count AS [Количество страниц]
FROM sys.dm_db_index_physical_stats(DB_ID(N'tempdb'), OBJECT_ID(N'tempdb..#EX_FRAG'), NULL, NULL, N'SAMPLED') AS ips
    INNER JOIN tempdb.sys.indexes AS i
        ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE i.name IS NOT NULL;
GO

ALTER INDEX IX_EX_FRAG_TKEY ON #EX_FRAG REORGANIZE;
GO

PRINT N'Задание 5. Фрагментация после REORGANIZE';
SELECT
    i.name AS [Индекс],
    ips.avg_fragmentation_in_percent AS [Фрагментация (%)],
    ips.page_count AS [Количество страниц]
FROM sys.dm_db_index_physical_stats(DB_ID(N'tempdb'), OBJECT_ID(N'tempdb..#EX_FRAG'), NULL, NULL, N'SAMPLED') AS ips
    INNER JOIN tempdb.sys.indexes AS i
        ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE i.name IS NOT NULL;
GO

ALTER INDEX IX_EX_FRAG_TKEY ON #EX_FRAG REBUILD WITH (ONLINE = OFF);
GO

PRINT N'Задание 5. Фрагментация после REBUILD';
SELECT
    i.name AS [Индекс],
    ips.avg_fragmentation_in_percent AS [Фрагментация (%)],
    ips.page_count AS [Количество страниц]
FROM sys.dm_db_index_physical_stats(DB_ID(N'tempdb'), OBJECT_ID(N'tempdb..#EX_FRAG'), NULL, NULL, N'SAMPLED') AS ips
    INNER JOIN tempdb.sys.indexes AS i
        ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE i.name IS NOT NULL;
GO

/* ============================================================
   6. Демонстрация параметра FILLFACTOR.
   ============================================================ */
PRINT N'Задание 6. FILLFACTOR';

DROP INDEX IX_EX_FRAG_TKEY ON #EX_FRAG;
GO

CREATE NONCLUSTERED INDEX IX_EX_FRAG_TKEY
ON #EX_FRAG(TKEY)
WITH (FILLFACTOR = 65);
GO

PRINT N'Задание 6. Фрагментация после создания индекса с FILLFACTOR = 65';
SELECT
    i.name AS [Индекс],
    i.fill_factor AS [FillFactor],
    ips.avg_page_space_used_in_percent AS [Среднее заполнение страниц (%)],
    ips.avg_fragmentation_in_percent AS [Фрагментация (%)],
    ips.page_count AS [Количество страниц]
FROM sys.dm_db_index_physical_stats(DB_ID(N'tempdb'), OBJECT_ID(N'tempdb..#EX_FRAG'), NULL, NULL, N'DETAILED') AS ips
    INNER JOIN tempdb.sys.indexes AS i
        ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE i.name IS NOT NULL;
GO

INSERT TOP (50) PERCENT INTO #EX_FRAG(TKEY, TF)
SELECT TKEY, TF
FROM #EX_FRAG
ORDER BY NEWID();
GO

PRINT N'Задание 6. Фрагментация после добавления 50% строк';
SELECT
    i.name AS [Индекс],
    i.fill_factor AS [FillFactor],
    ips.avg_page_space_used_in_percent AS [Среднее заполнение страниц (%)],
    ips.avg_fragmentation_in_percent AS [Фрагментация (%)],
    ips.page_count AS [Количество страниц]
FROM sys.dm_db_index_physical_stats(DB_ID(N'tempdb'), OBJECT_ID(N'tempdb..#EX_FRAG'), NULL, NULL, N'DETAILED') AS ips
    INNER JOIN tempdb.sys.indexes AS i
        ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE i.name IS NOT NULL;
GO

SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
GO
