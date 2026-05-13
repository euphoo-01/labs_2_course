PRINT N'--- Задание 9 ---';

DECLARE @EntId_Source uniqueidentifier = (SELECT TOP 1 [ID предприятия] FROM [Предприятия] WHERE [Название предприятия] = N'ООО "Рай"');
DECLARE @EntId_Dest uniqueidentifier = (SELECT TOP 1 [ID предприятия] FROM [Предприятия] WHERE [Название предприятия] = N'ЗАО "ДВШиман"');
DECLARE @TypeId uniqueidentifier = (SELECT TOP 1 [ID типа] FROM [Типы показателей] WHERE [Название показателя] = N'Чистая прибыль');

DECLARE @TransferAmount decimal(18,2) = 15000.00;

BEGIN TRY
BEGIN TRAN MyBaseTransaction;
    PRINT N'Транзакция перевода начата.';

UPDATE [Значения показателей]
SET [Значение] = [Значение] - @TransferAmount
WHERE [ID предприятия] = @EntId_Source AND [ID типа показателя] = @TypeId;
PRINT N'Успешно списано: ' + CAST(@TransferAmount AS nvarchar(20));

    SAVE TRAN BeforeDeposit;

UPDATE [Значения показателей]
SET [Значение] = [Значение] + @TransferAmount
WHERE [ID предприятия] = @EntId_Dest AND [ID типа показателя] = @TypeId;

-- DECLARE @Err int = 1 / 0; 

PRINT N'Успешно зачислено: ' + CAST(@TransferAmount AS nvarchar(20));

COMMIT TRAN MyBaseTransaction;
PRINT N'Транзакция успешно зафиксирована (COMMIT).';

END TRY
BEGIN CATCH
PRINT N'ВНИМАНИЕ! Произошла ошибка во время перевода: ' + ERROR_MESSAGE();
    
    IF @@TRANCOUNT > 0
BEGIN
ROLLBACK TRAN MyBaseTransaction;
PRINT N'Был выполнен полный ROLLBACK. Изменения отменены, данные в безопасности.';
END
END CATCH;
GO

PRINT N'';
PRINT N'--- Пример 2: Установка уровня изолированности ---';

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

BEGIN TRAN;
    PRINT N'Формирование отчета по показателям...';
SELECT E.[Название предприятия], T.[Название показателя], Z.[Значение]
FROM [Значения показателей] Z
    INNER JOIN [Предприятия] E ON Z.[ID предприятия] = E.[ID предприятия]
    INNER JOIN [Типы показателей] T ON Z.[ID типа показателя] = T.[ID типа];
COMMIT TRAN;
PRINT N'Отчет успешно сформирован.';
GO