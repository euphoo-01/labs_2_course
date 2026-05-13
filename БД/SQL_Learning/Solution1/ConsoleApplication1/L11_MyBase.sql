PRINT N'--- ПРИМЕР 1: Список всех предприятий (аналог Задания 1) ---';
DECLARE @ent_name nvarchar(30);
DECLARE @ent_list nvarchar(max) = N'';

DECLARE EntCursor CURSOR FOR SELECT [Название предприятия] FROM [Предприятия];
OPEN EntCursor;
FETCH EntCursor INTO @ent_name;
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @ent_list = @ent_list + RTRIM(@ent_name) + N', ';
FETCH EntCursor INTO @ent_name;
END
CLOSE EntCursor; DEALLOCATE EntCursor;

IF LEN(@ent_list) > 0 SET @ent_list = LEFT(@ent_list, LEN(@ent_list) - 2);
PRINT N'Наши предприятия: ' + @ent_list;
PRINT N'';
GO


PRINT N'--- ПРИМЕР 2: Навигация SCROLL по типам показателей (аналог Задания 4) ---';
DECLARE @type_name nvarchar(40);
DECLARE ScrollTypeCursor CURSOR LOCAL SCROLL FOR SELECT [Название показателя] FROM [Типы показателей];

OPEN ScrollTypeCursor;
FETCH LAST FROM ScrollTypeCursor INTO @type_name;
PRINT N'Последний добавленный тип показателя: ' + @type_name;
FETCH FIRST FROM ScrollTypeCursor INTO @type_name;
PRINT N'Первый тип показателя: ' + @type_name;
CLOSE ScrollTypeCursor; DEALLOCATE ScrollTypeCursor;
PRINT N'';
GO


PRINT N'--- ПРИМЕР 3: Использование CURRENT OF (Аналог Задания 5) ---';
DECLARE @val decimal(18,2);
DECLARE UpdateValCursor CURSOR LOCAL DYNAMIC FOR
SELECT [Значение] FROM [Значения показателей] WHERE [Значение] < 1000 FOR UPDATE;

OPEN UpdateValCursor;
FETCH NEXT FROM UpdateValCursor INTO @val;

WHILE @@FETCH_STATUS = 0
BEGIN
UPDATE [Значения показателей]
SET [Значение] = @val * 1.10
WHERE CURRENT OF UpdateValCursor;

PRINT N'Значение ' + CAST(@val AS nvarchar) + N' было успешно обновлено через курсор.';
FETCH NEXT FROM UpdateValCursor INTO @val;
END

CLOSE UpdateValCursor; DEALLOCATE UpdateValCursor;
GO