IF OBJECT_ID(N'dbo.[Дисциплины]', N'V') IS NOT NULL DROP VIEW dbo.[Дисциплины];
GO
IF OBJECT_ID(N'dbo.[Лекционные_аудитории]', N'V') IS NOT NULL DROP VIEW dbo.[Лекционные_аудитории];
GO
IF OBJECT_ID(N'dbo.[Аудитории]', N'V') IS NOT NULL DROP VIEW dbo.[Аудитории];
GO
IF OBJECT_ID(N'dbo.[Количество_кафедр]', N'V') IS NOT NULL DROP VIEW dbo.[Количество_кафедр];
GO
IF OBJECT_ID(N'dbo.[Преподаватель]', N'V') IS NOT NULL DROP VIEW dbo.[Преподаватель];
GO

CREATE VIEW dbo.[Преподаватель]
AS
SELECT
    T.TEACHER      AS [Код],
        T.TEACHER_NAME AS [Имя преподавателя],
        T.GENDER       AS [Пол],
        T.PULPIT       AS [Код кафедры]
FROM dbo.TEACHER AS T;
GO
SELECT * FROM dbo.[Преподаватель];
GO

CREATE VIEW dbo.[Количество_кафедр]
AS
SELECT
    F.FACULTY_NAME AS [Факультет],
        COUNT(P.PULPIT) AS [Количество кафедр]
FROM dbo.FACULTY AS F
    LEFT JOIN dbo.PULPIT AS P ON P.FACULTY = F.FACULTY
GROUP BY F.FACULTY_NAME;
GO
SELECT * FROM dbo.[Количество_кафедр];
GO

CREATE VIEW dbo.[Аудитории]
AS
SELECT
    A.AUDITORIUM      AS [Код],
        A.AUDITORIUM_NAME AS [Наименование аудитории]
FROM dbo.AUDITORIUM AS A
WHERE A.AUDITORIUM_TYPE LIKE N'ЛК%';
GO
SELECT * FROM dbo.[Аудитории];
GO

BEGIN TRANSACTION;
UPDATE dbo.[Аудитории]
SET [Наименование аудитории] = [Наименование аудитории]
WHERE [Код] IN (SELECT TOP (1) [Код] FROM dbo.[Аудитории]);
ROLLBACK TRANSACTION;
GO

CREATE VIEW dbo.[Лекционные_аудитории]
AS
SELECT
    A.AUDITORIUM      AS [Код],
        A.AUDITORIUM_NAME AS [Наименование аудитории]
FROM dbo.AUDITORIUM AS A
WHERE A.AUDITORIUM_TYPE LIKE N'ЛК%'
WITH CHECK OPTION;
GO
SELECT * FROM dbo.[Лекционные_аудитории];
GO

CREATE VIEW dbo.[Дисциплины]
AS
SELECT TOP (100) PERCENT
    S.SUBJECT      AS [Код],
        S.SUBJECT_NAME AS [Наименование дисциплины],
        S.PULPIT       AS [Код кафедры]
FROM dbo.SUBJECT AS S
ORDER BY S.SUBJECT_NAME;
GO
SELECT * FROM dbo.[Дисциплины]
ORDER BY [Наименование дисциплины];
GO

ALTER VIEW dbo.[Количество_кафедр]
WITH SCHEMABINDING
AS
SELECT
    F.FACULTY_NAME AS [Факультет],
        COUNT_BIG(*)   AS [Количество кафедр]
FROM dbo.FACULTY AS F
    INNER JOIN dbo.PULPIT AS P ON P.FACULTY = F.FACULTY
GROUP BY F.FACULTY_NAME;
GO
SELECT * FROM dbo.[Количество_кафедр];
GO

SELECT
    OBJECT_SCHEMA_NAME(V.object_id) AS [Схема],
    V.name AS [Представление],
    M.is_schema_bound AS [Привязано к схеме]
FROM sys.views AS V
    INNER JOIN sys.sql_modules AS M ON M.object_id = V.object_id
WHERE V.name = N'Количество_кафедр';
GO

BEGIN TRY
EXEC(N'ALTER TABLE dbo.PULPIT DROP COLUMN FACULTY;');
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH;
GO
