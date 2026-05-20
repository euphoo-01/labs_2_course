USE [Лавшук_MyBase]
GO

DROP PROCEDURE IF EXISTS [dbo].[PVALUE_INSERTX]
    GO
DROP PROCEDURE IF EXISTS [dbo].[PVALUE_INSERT]
    GO
DROP PROCEDURE IF EXISTS [dbo].[PINDICATOR_REPORT]
    GO
DROP PROCEDURE IF EXISTS [dbo].[PENTERPRISE_INSERT]
    GO
DROP PROCEDURE IF EXISTS [dbo].[PINDICATOR_TYPES]
    GO

CREATE PROCEDURE [dbo].[PINDICATOR_TYPES]
AS
BEGIN
    SET NOCOUNT ON;
SELECT [ID типа], [Важность], [Название показателя]
FROM [dbo].[Типы показателей]
ORDER BY [Важность], [Название показателя];
RETURN @@ROWCOUNT;
END
GO

DECLARE @rc INT;
EXEC @rc = [dbo].[PINDICATOR_TYPES];
PRINT N'Количество строк = ' + CAST(@rc AS NVARCHAR(20));
GO

ALTER PROCEDURE [dbo].[PINDICATOR_TYPES]
    @p NVARCHAR(15) = NULL,
    @c INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @total INT;
SELECT [ID типа], [Важность], [Название показателя]
FROM [dbo].[Типы показателей]
WHERE @p IS NULL OR [Важность] = @p
ORDER BY [Название показателя];
SET @c = @@ROWCOUNT;
SELECT @total = COUNT(*) FROM [dbo].[Типы показателей];
RETURN @total;
END
GO

DECLARE @rc INT, @c INT, @importance NVARCHAR(15);
SELECT TOP (1) @importance = [Важность] FROM [dbo].[Типы показателей] ORDER BY [Важность];
EXEC @rc = [dbo].[PINDICATOR_TYPES] @p = @importance, @c = @c OUTPUT;
PRINT N'Количество строк по важности = ' + CAST(@c AS NVARCHAR(20));
PRINT N'Общее количество типов показателей = ' + CAST(@rc AS NVARCHAR(20));
GO

ALTER PROCEDURE [dbo].[PINDICATOR_TYPES]
    @p NVARCHAR(15) = NULL
AS
BEGIN
    SET NOCOUNT ON;
SELECT [ID типа], [Важность], [Название показателя]
FROM [dbo].[Типы показателей]
WHERE @p IS NULL OR [Важность] = @p
ORDER BY [Название показателя];
RETURN @@ROWCOUNT;
END
GO

CREATE TABLE #INDICATOR_TYPES
(
    [ID типа] UNIQUEIDENTIFIER NOT NULL,
    [Важность] NVARCHAR(15) NULL,
    [Название показателя] NVARCHAR(40) NULL
);
DECLARE @importance1 NVARCHAR(15), @importance2 NVARCHAR(15);
SELECT TOP (1) @importance1 = [Важность] FROM [dbo].[Типы показателей] ORDER BY [Важность];
SELECT TOP (1) @importance2 = [Важность] FROM [dbo].[Типы показателей] WHERE [Важность] <> @importance1 ORDER BY [Важность];
INSERT INTO #INDICATOR_TYPES EXEC [dbo].[PINDICATOR_TYPES] @p = @importance1;
INSERT INTO #INDICATOR_TYPES EXEC [dbo].[PINDICATOR_TYPES] @p = @importance2;
SELECT * FROM #INDICATOR_TYPES ORDER BY [Важность], [Название показателя];
DROP TABLE #INDICATOR_TYPES;
GO

CREATE PROCEDURE [dbo].[PENTERPRISE_INSERT]
    @name NVARCHAR(30),
    @account NVARCHAR(30),
    @bic NVARCHAR(30),
    @phone NVARCHAR(30),
    @contact NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
BEGIN TRY
INSERT INTO [dbo].[Предприятия]
            ([Название предприятия], [Расчетный счет], [БИК банка], [Телефон], [Контактное лицо])
        VALUES
            (@name, @account, @bic, @phone, @contact);
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

DECLARE @rc INT;
IF NOT EXISTS (SELECT 1 FROM [dbo].[Предприятия] WHERE [Название предприятия] = N'Тест Л13')
BEGIN
EXEC @rc = [dbo].[PENTERPRISE_INSERT] @name = N'Тест Л13', @account = N'BY00TEST0000000000000000', @bic = N'TESTBY2X', @phone = N'+375291111111', @contact = N'Иванов И.И.';
END
ELSE
BEGIN
    SET @rc = 1;
END;
PRINT N'код возврата = ' + CAST(@rc AS NVARCHAR(20));
GO

CREATE PROCEDURE [dbo].[PINDICATOR_REPORT]
    @p NVARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @rc INT = 0;
    DECLARE @indicator NVARCHAR(40);
    DECLARE @text NVARCHAR(MAX) = N'';

    DECLARE IndicatorCursor CURSOR LOCAL FAST_FORWARD FOR
SELECT [Название показателя]
FROM [dbo].[Типы показателей]
WHERE [Важность] = @p
ORDER BY [Название показателя];

BEGIN TRY
IF NOT EXISTS (SELECT 1 FROM [dbo].[Типы показателей] WHERE [Важность] = @p)
            RAISERROR(N'ошибка в параметрах', 11, 1);

OPEN IndicatorCursor;
FETCH NEXT FROM IndicatorCursor INTO @indicator;
WHILE @@FETCH_STATUS = 0
BEGIN
            SET @text = @text + CASE WHEN LEN(@text) = 0 THEN N'' ELSE N', ' END + RTRIM(@indicator);
            SET @rc = @rc + 1;
FETCH NEXT FROM IndicatorCursor INTO @indicator;
END;
CLOSE IndicatorCursor;
DEALLOCATE IndicatorCursor;

        PRINT N'Показатели с важностью ' + @p;
        PRINT @text;
RETURN @rc;
END TRY
BEGIN CATCH
IF CURSOR_STATUS('local', 'IndicatorCursor') >= 0 CLOSE IndicatorCursor;
        IF CURSOR_STATUS('local', 'IndicatorCursor') >= -1 DEALLOCATE IndicatorCursor;
        PRINT N'ошибка в параметрах';
        IF ERROR_PROCEDURE() IS NOT NULL PRINT N'имя процедуры : ' + ERROR_PROCEDURE();
RETURN @rc;
END CATCH
END
GO

DECLARE @rc INT, @importance NVARCHAR(15);
SELECT TOP (1) @importance = [Важность] FROM [dbo].[Типы показателей] ORDER BY [Важность];
EXEC @rc = [dbo].[PINDICATOR_REPORT] @p = @importance;
PRINT N'количество показателей = ' + CAST(@rc AS NVARCHAR(20));
EXEC @rc = [dbo].[PINDICATOR_REPORT] @p = N'нет';
PRINT N'количество показателей = ' + CAST(@rc AS NVARCHAR(20));
GO

CREATE PROCEDURE [dbo].[PVALUE_INSERT]
    @enterpriseId UNIQUEIDENTIFIER,
    @typeId UNIQUEIDENTIFIER,
    @value DECIMAL(18,2),
    @date DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
BEGIN TRY
INSERT INTO [dbo].[Значения показателей]
            ([ID предприятия], [ID типа показателя], [Значение], [Дата])
        VALUES
            (@enterpriseId, @typeId, @value, ISNULL(@date, CONVERT(DATE, GETDATE())));
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

CREATE PROCEDURE [dbo].[PVALUE_INSERTX]
    @enterpriseId UNIQUEIDENTIFIER,
    @typeId UNIQUEIDENTIFIER,
    @importance NVARCHAR(15),
    @indicatorName NVARCHAR(40),
    @value DECIMAL(18,2),
    @date DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @rc INT = 1;
BEGIN TRY
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;

INSERT INTO [dbo].[Типы показателей]
([ID типа], [Важность], [Название показателя])
VALUES
    (@typeId, @importance, @indicatorName);

EXEC @rc = [dbo].[PVALUE_INSERT]
            @enterpriseId = @enterpriseId,
            @typeId = @typeId,
            @value = @value,
            @date = @date;

        IF @rc <> 1 RAISERROR(N'ошибка добавления значения показателя', 11, 1);

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

DECLARE @enterpriseId UNIQUEIDENTIFIER;
SELECT TOP (1) @enterpriseId = [ID предприятия] FROM [dbo].[Предприятия] ORDER BY [Название предприятия];
IF @enterpriseId IS NULL
BEGIN
INSERT INTO [dbo].[Предприятия]
([Название предприятия], [Расчетный счет], [БИК банка], [Телефон], [Контактное лицо])
VALUES
    (N'Тест Л13', N'BY00TEST0000000000000000', N'TESTBY2X', N'+375291111111', N'Иванов И.И.');
SELECT @enterpriseId = [ID предприятия] FROM [dbo].[Предприятия] WHERE [Название предприятия] = N'Тест Л13';
END;
DECLARE @oldTypeId UNIQUEIDENTIFIER;
SELECT @oldTypeId = [ID типа] FROM [dbo].[Типы показателей] WHERE [Название показателя] = N'Тестовый показатель L13';
IF @oldTypeId IS NOT NULL DELETE FROM [dbo].[Значения показателей] WHERE [ID типа показателя] = @oldTypeId;
DELETE FROM [dbo].[Типы показателей] WHERE [Название показателя] = N'Тестовый показатель L13';
DECLARE @typeId UNIQUEIDENTIFIER = NEWID();
DECLARE @rc INT;
EXEC @rc = [dbo].[PVALUE_INSERTX]
    @enterpriseId = @enterpriseId,
    @typeId = @typeId,
    @importance = N'высокая',
    @indicatorName = N'Тестовый показатель L13',
    @value = 12345.67,
    @date = '20260520';
PRINT N'код возврата = ' + CAST(@rc AS NVARCHAR(20));
EXEC @rc = [dbo].[PVALUE_INSERTX]
    @enterpriseId = @enterpriseId,
    @typeId = @typeId,
    @importance = N'высокая',
    @indicatorName = N'Тестовый показатель L13',
    @value = 12345.67,
    @date = '20260520';
PRINT N'код возврата = ' + CAST(@rc AS NVARCHAR(20));
GO
