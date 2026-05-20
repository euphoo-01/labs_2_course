USE [UNIVER]
GO

DROP PROCEDURE IF EXISTS [dbo].[PRINT_REPORT]
    GO
DROP PROCEDURE IF EXISTS [dbo].[PAUDITORIUM_INSERTX]
    GO
DROP PROCEDURE IF EXISTS [dbo].[SUBJECT_REPORT]
    GO
DROP PROCEDURE IF EXISTS [dbo].[PAUDITORIUM_INSERT]
    GO
DROP PROCEDURE IF EXISTS [dbo].[PSUBJECT]
    GO

CREATE PROCEDURE [dbo].[PSUBJECT]
AS
BEGIN
    SET NOCOUNT ON;
SELECT [SUBJECT], [SUBJECT_NAME], [PULPIT]
FROM [dbo].[SUBJECT]
ORDER BY [PULPIT], [SUBJECT];
RETURN @@ROWCOUNT;
END
GO

DECLARE @rc INT;
EXEC @rc = [dbo].[PSUBJECT];
PRINT N'Количество строк = ' + CAST(@rc AS NVARCHAR(20));
GO

ALTER PROCEDURE [dbo].[PSUBJECT]
    @p VARCHAR(20) = NULL,
    @c INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @total INT;
SELECT [SUBJECT], [SUBJECT_NAME], [PULPIT]
FROM [dbo].[SUBJECT]
WHERE @p IS NULL OR RTRIM([PULPIT]) = RTRIM(@p)
ORDER BY [SUBJECT];
SET @c = @@ROWCOUNT;
SELECT @total = COUNT(*) FROM [dbo].[SUBJECT];
RETURN @total;
END
GO

DECLARE @rc INT, @c INT;
EXEC @rc = [dbo].[PSUBJECT] @p = 'ИСиТ', @c = @c OUTPUT;
PRINT N'Количество строк по кафедре = ' + CAST(@c AS NVARCHAR(20));
PRINT N'Общее количество дисциплин = ' + CAST(@rc AS NVARCHAR(20));
GO

ALTER PROCEDURE [dbo].[PSUBJECT]
    @p VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
SELECT [SUBJECT], [SUBJECT_NAME], [PULPIT]
FROM [dbo].[SUBJECT]
WHERE @p IS NULL OR RTRIM([PULPIT]) = RTRIM(@p)
ORDER BY [SUBJECT];
RETURN @@ROWCOUNT;
END
GO

CREATE TABLE #SUBJECT
(
    [SUBJECT] CHAR(10) NOT NULL,
    [SUBJECT_NAME] VARCHAR(100) NULL,
    [PULPIT] CHAR(20) NOT NULL
);
INSERT INTO #SUBJECT EXEC [dbo].[PSUBJECT] @p = 'ИСиТ';
INSERT INTO #SUBJECT EXEC [dbo].[PSUBJECT] @p = 'ТНХСиППМ';
SELECT * FROM #SUBJECT ORDER BY [PULPIT], [SUBJECT];
DROP TABLE #SUBJECT;
GO

CREATE PROCEDURE [dbo].[PAUDITORIUM_INSERT]
    @a CHAR(20),
    @n VARCHAR(50),
    @c INT = 0,
    @t CHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
BEGIN TRY
INSERT INTO [dbo].[AUDITORIUM]
            ([AUDITORIUM], [AUDITORIUM_NAME], [AUDITORIUM_CAPACITY], [AUDITORIUM_TYPE])
        VALUES
            (@a, @n, @c, @t);
RETURN 1;
END TRY
BEGIN CATCH
PRINT N'номер ошибки  : ' + CAST(ERROR_NUMBER() AS NVARCHAR(20));
        PRINT N'уровень       : ' + CAST(ERROR_SEVERITY() AS NVARCHAR(20));
        PRINT N'сообщение     : ' + ERROR_MESSAGE();
RETURN -1;
END CATCH
END
GO

DELETE FROM [dbo].[AUDITORIUM] WHERE RTRIM([AUDITORIUM]) = '999-1';
DECLARE @rc INT;
EXEC @rc = [dbo].[PAUDITORIUM_INSERT] @a = '999-1', @n = '999-1', @c = 45, @t = 'ЛК';
PRINT N'код возврата = ' + CAST(@rc AS NVARCHAR(20));
EXEC @rc = [dbo].[PAUDITORIUM_INSERT] @a = '999-1', @n = '999-1', @c = 45, @t = 'ЛК';
PRINT N'код возврата = ' + CAST(@rc AS NVARCHAR(20));
GO

CREATE PROCEDURE [dbo].[SUBJECT_REPORT]
    @p CHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @rc INT = 0;
    DECLARE @subject CHAR(10);
    DECLARE @text NVARCHAR(MAX) = N'';

    DECLARE SubjectCursor CURSOR LOCAL FAST_FORWARD FOR
SELECT [SUBJECT]
FROM [dbo].[SUBJECT]
WHERE RTRIM([PULPIT]) = RTRIM(@p)
ORDER BY [SUBJECT];

BEGIN TRY
IF NOT EXISTS (SELECT 1 FROM [dbo].[SUBJECT] WHERE RTRIM([PULPIT]) = RTRIM(@p))
            RAISERROR(N'ошибка в параметрах', 11, 1);

OPEN SubjectCursor;
FETCH NEXT FROM SubjectCursor INTO @subject;
WHILE @@FETCH_STATUS = 0
BEGIN
            SET @text = @text + CASE WHEN LEN(@text) = 0 THEN N'' ELSE N', ' END + RTRIM(@subject);
            SET @rc = @rc + 1;
FETCH NEXT FROM SubjectCursor INTO @subject;
END;
CLOSE SubjectCursor;
DEALLOCATE SubjectCursor;

        PRINT N'Дисциплины кафедры ' + RTRIM(@p);
        PRINT @text;
RETURN @rc;
END TRY
BEGIN CATCH
IF CURSOR_STATUS('local', 'SubjectCursor') >= 0 CLOSE SubjectCursor;
        IF CURSOR_STATUS('local', 'SubjectCursor') >= -1 DEALLOCATE SubjectCursor;
        PRINT N'ошибка в параметрах';
        IF ERROR_PROCEDURE() IS NOT NULL PRINT N'имя процедуры : ' + ERROR_PROCEDURE();
RETURN @rc;
END CATCH
END
GO

DECLARE @rc INT;
EXEC @rc = [dbo].[SUBJECT_REPORT] @p = 'ИСиТ';
PRINT N'количество дисциплин = ' + CAST(@rc AS NVARCHAR(20));
EXEC @rc = [dbo].[SUBJECT_REPORT] @p = 'BAD';
PRINT N'количество дисциплин = ' + CAST(@rc AS NVARCHAR(20));
GO

CREATE PROCEDURE [dbo].[PAUDITORIUM_INSERTX]
    @a CHAR(20),
    @n VARCHAR(50),
    @c INT = 0,
    @t CHAR(10),
    @tn VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @rc INT = 1;
BEGIN TRY
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;

INSERT INTO [dbo].[AUDITORIUM_TYPE]
([AUDITORIUM_TYPE], [AUDITORIUM_TYPENAME])
VALUES
    (@t, @tn);

EXEC @rc = [dbo].[PAUDITORIUM_INSERT] @a = @a, @n = @n, @c = @c, @t = @t;
        IF @rc <> 1 RAISERROR(N'ошибка добавления аудитории', 11, 1);

COMMIT TRANSACTION;
RETURN 1;
END TRY
BEGIN CATCH
PRINT N'номер ошибки  : ' + CAST(ERROR_NUMBER() AS NVARCHAR(20));
        PRINT N'сообщение     : ' + ERROR_MESSAGE();
        PRINT N'уровень       : ' + CAST(ERROR_SEVERITY() AS NVARCHAR(20));
        PRINT N'метка         : ' + CAST(ERROR_STATE() AS NVARCHAR(20));
        PRINT N'номер строки  : ' + CAST(ERROR_LINE() AS NVARCHAR(20));
        IF ERROR_PROCEDURE() IS NOT NULL PRINT N'имя процедуры : ' + ERROR_PROCEDURE();
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
RETURN -1;
END CATCH
END
GO

DELETE FROM [dbo].[AUDITORIUM] WHERE RTRIM([AUDITORIUM]) = '999-2';
DELETE FROM [dbo].[AUDITORIUM_TYPE] WHERE RTRIM([AUDITORIUM_TYPE]) = 'ТЕСТ';
DECLARE @rc INT;
EXEC @rc = [dbo].[PAUDITORIUM_INSERTX] @a = '999-2', @n = '999-2', @c = 50, @t = 'ТЕСТ', @tn = 'Тестовая';
PRINT N'код возврата = ' + CAST(@rc AS NVARCHAR(20));
EXEC @rc = [dbo].[PAUDITORIUM_INSERTX] @a = '999-2', @n = '999-2', @c = 50, @t = 'ТЕСТ', @tn = 'Тестовая';
PRINT N'код возврата = ' + CAST(@rc AS NVARCHAR(20));
GO

CREATE PROCEDURE [dbo].[PRINT_REPORT]
    @f CHAR(10) = NULL,
    @p CHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @rc INT = 0;
    DECLARE @faculty CHAR(10);
    DECLARE @facultyName VARCHAR(50);
    DECLARE @pulpit CHAR(20);
    DECLARE @pulpitName VARCHAR(100);
    DECLARE @subjectCount INT;
    DECLARE @subjects NVARCHAR(MAX);

BEGIN TRY
IF @f IS NULL AND @p IS NOT NULL
SELECT @f = [FACULTY]
FROM [dbo].[PULPIT]
WHERE RTRIM([PULPIT]) = RTRIM(@p);

IF @f IS NULL
            RAISERROR(N'ошибка в параметрах', 11, 1);

        IF @p IS NOT NULL AND NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[PULPIT]
            WHERE RTRIM([PULPIT]) = RTRIM(@p) AND RTRIM([FACULTY]) = RTRIM(@f)
        )
            RAISERROR(N'ошибка в параметрах', 11, 1);

        IF NOT EXISTS (SELECT 1 FROM [dbo].[FACULTY] WHERE RTRIM([FACULTY]) = RTRIM(@f))
            RAISERROR(N'ошибка в параметрах', 11, 1);

SELECT @faculty = [FACULTY], @facultyName = [FACULTY_NAME]
FROM [dbo].[FACULTY]
WHERE RTRIM([FACULTY]) = RTRIM(@f);

PRINT N'Факультет: ' + RTRIM(@faculty) + N' ' + ISNULL(@facultyName, N'');

        DECLARE ReportCursor CURSOR LOCAL FAST_FORWARD FOR
SELECT [PULPIT], [PULPIT_NAME]
FROM [dbo].[PULPIT]
WHERE RTRIM([FACULTY]) = RTRIM(@f)
  AND (@p IS NULL OR RTRIM([PULPIT]) = RTRIM(@p))
ORDER BY [PULPIT];

OPEN ReportCursor;
FETCH NEXT FROM ReportCursor INTO @pulpit, @pulpitName;
WHILE @@FETCH_STATUS = 0
BEGIN
SELECT @subjectCount = COUNT(*)
FROM [dbo].[SUBJECT]
WHERE RTRIM([PULPIT]) = RTRIM(@pulpit);

SELECT @subjects = COALESCE(@subjects + N', ', N'') + RTRIM([SUBJECT])
FROM [dbo].[SUBJECT]
WHERE RTRIM([PULPIT]) = RTRIM(@pulpit);

PRINT N'Кафедра: ' + RTRIM(@pulpit) + N' ' + ISNULL(@pulpitName, N'');
            PRINT N'Количество дисциплин: ' + CAST(@subjectCount AS NVARCHAR(20));
            PRINT ISNULL(@subjects, N'Дисциплины отсутствуют');
            SET @subjects = NULL;
            SET @rc = @rc + 1;
FETCH NEXT FROM ReportCursor INTO @pulpit, @pulpitName;
END;
CLOSE ReportCursor;
DEALLOCATE ReportCursor;
RETURN @rc;
END TRY
BEGIN CATCH
IF CURSOR_STATUS('local', 'ReportCursor') >= 0 CLOSE ReportCursor;
        IF CURSOR_STATUS('local', 'ReportCursor') >= -1 DEALLOCATE ReportCursor;
        PRINT N'ошибка в параметрах';
RETURN @rc;
END CATCH
END
GO

BEGIN TRY
    DECLARE @rc INT;
EXEC @rc = [dbo].[PRINT_REPORT] @f = 'ИТ', @p = NULL;
    PRINT N'количество кафедр = ' + CAST(@rc AS NVARCHAR(20));
EXEC @rc = [dbo].[PRINT_REPORT] @f = NULL, @p = 'ИСиТ';
    PRINT N'количество кафедр = ' + CAST(@rc AS NVARCHAR(20));
EXEC @rc = [dbo].[PRINT_REPORT] @f = NULL, @p = 'BAD';
    PRINT N'количество кафедр = ' + CAST(@rc AS NVARCHAR(20));
END TRY
BEGIN CATCH
PRINT ERROR_MESSAGE();
END CATCH
GO
